using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleSellBooks_API.DTOs.Auth;
using SimpleSellBooks_BusinessLayer.people;
using SimpleSellBooks_DataLayer.people;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace SimpleSellBooks_API.Controllers.Authentication
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // This endpoint handles user login.
        // It verifies credentials and returns a JWT token if login succeeds.
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] clsSignInDTO signInDTO)
        {
            // Step 1: Find the person by email from DB.
            // Email acts as the unique login identifier.
            clsSignInResponseDTO? signInResp = clsPersonBusiness.Login(signInDTO);

            // If no student is found with the given email,
            // return 401 Unauthorized without revealing which field was wrong.
            if (signInResp == null)
                return Unauthorized("Invalid credentials");


            try
            {
                // Step 2: Create claims that represent the authenticated user's identity.
                // These claims will be embedded inside the JWT.
                var claims = new[]
                {
                    // Unique identifier for the student
                    new Claim(ClaimTypes.NameIdentifier, signInResp.personID.ToString()),


                    // Student email address
                    new Claim(ClaimTypes.Email, signInResp.email),


                    // Role (Student or Admin) used later for authorization
                    new Claim(ClaimTypes.Role, signInResp.role ?? "Unkown")
                };


                // Step 3: Create the symmetric security key used to sign the JWT.
                // This key must match the key used in JWT validation middleware.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));


                // Step 4: Define the signing credentials.
                // This specifies the algorithm used to sign the token.
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                // Step 5: Create the JWT token.
                // The token includes issuer, audience, claims, expiration, and signature.
                var token = new JwtSecurityToken(
                    issuer: "StoreBookApi",
                    audience: "StoreApiBooks",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                //Step 6: Generate JWT Access Token
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                // Create refresh token (random)
                var refreshToken = GenerateRefreshToken();

                string refreshTokenHashHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

                DateTime refreshTokenExpiresAt =
                    DateTime.UtcNow.AddDays(7);

                clsUpdateRefreshTokenDto refreshDto = new clsUpdateRefreshTokenDto();
                refreshDto.personId = signInResp.personID;
                refreshDto.RefreshTokenHash = refreshTokenHashHash;
                refreshDto.RefreshTokenExpirationDate = refreshTokenExpiresAt;

                bool isUpdated = clsPersonBusiness.UpdateRefreshToken(refreshDto);

                if (!isUpdated)
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        "Failed to update refresh token."
                    );
                }


                signInResp.AccessToken = accessToken;

                signInResp.RefreshToken = refreshToken;

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : Login Auth " + ex.Message);
            }

            // Step 7: Return the serialized JWT token to the client.
            // The client will send this token with future requests.
            return Ok(signInResp);
        }


       [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] clsRefreshRequest request)
        {
            clsSignInResponseDTO? refreshTokenResponse = clsPersonBusiness.RefreshTokenResponce(request.Email);

            clsTokenResponse tokenResponse = new clsTokenResponse();

            if (refreshTokenResponse == null)
                return Unauthorized("Invalid refresh request");

            if (refreshTokenResponse.RefreshTokenRevokedAt != null)
                return Unauthorized("Refresh token is revoked");

            if (refreshTokenResponse.RefreshTokenExpirationDate == null || 
                refreshTokenResponse.RefreshTokenExpirationDate <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, refreshTokenResponse.RefreshTokenHash);
            
            if (!refreshValid)
                return Unauthorized("Invalid refresh token");

            // Issue NEW access token (same claims & signing settings as login)
            try
            {
                // Create claims that represent the authenticated user's identity.
                // These claims will be embedded inside the JWT.
                var claims = new[]
                {
                    // Unique identifier for the student
                    new Claim(ClaimTypes.NameIdentifier, refreshTokenResponse.personID.ToString()),


                    // Student email address
                    new Claim(ClaimTypes.Email, refreshTokenResponse.email),


                    // Role (Student or Admin) used later for authorization
                    new Claim(ClaimTypes.Role, refreshTokenResponse.role ?? "Unkown")
                };


                // Create the symmetric security key used to sign the JWT.
                // This key must match the key used in JWT validation middleware.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));


                // Define the signing credentials.
                // This specifies the algorithm used to sign the token.
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                //Create the JWT token.
                // The token includes issuer, audience, claims, expiration, and signature.
                var token = new JwtSecurityToken(
                    issuer: "StoreBookApi",
                    audience: "StoreApiBooks",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                //Generate JWT Access Token
                var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);

                // Create refresh token (random)
                var newRefreshToken = GenerateRefreshToken();

                string newRefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);

                DateTime newRefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

                clsUpdateRefreshTokenDto newRefreshDto = new clsUpdateRefreshTokenDto();
                newRefreshDto.personId = refreshTokenResponse.personID;
                newRefreshDto.RefreshTokenHash = newRefreshTokenHash;
                newRefreshDto.RefreshTokenExpirationDate = newRefreshTokenExpiresAt;

                bool refreshTokenIsUpdated = clsPersonBusiness.UpdateRefreshToken(newRefreshDto);

                if (!refreshTokenIsUpdated)
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        "Failed to update refresh token."
                    );
                }


                tokenResponse.AccessToken = newAccessToken;

                tokenResponse.RefreshToken = newRefreshToken;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : Refresh Auth " + ex.Message);
            }
            return Ok(tokenResponse);
        }
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

    }
}
