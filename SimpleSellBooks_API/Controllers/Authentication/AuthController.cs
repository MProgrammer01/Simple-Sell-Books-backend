using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleSellBooks_BusinessLayer.people;
using SimpleSellBooks_DataLayer.people;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public IActionResult Login([FromBody] clsSignInDTO signInDTO)
        {
            // Step 1: Find the person by email from DB.
            // Email acts as the unique login identifier.
            clsSignInResponseDTO? signInResp = clsPersonBusiness.Login(signInDTO);

            // If no student is found with the given email,
            // return 401 Unauthorized without revealing which field was wrong.
            if (signInResp == null)
                return Unauthorized("Invalid credentials");


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
            //Step 6: Generate JWT
            signInResp.token = new JwtSecurityTokenHandler().WriteToken(token);
            
            // Step 7: Return the serialized JWT token to the client.
            // The client will send this token with future requests.
            return Ok(signInResp);
        }
    }
}
