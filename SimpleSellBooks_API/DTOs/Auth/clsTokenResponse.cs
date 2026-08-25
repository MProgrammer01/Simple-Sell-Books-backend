namespace SimpleSellBooks_API.DTOs.Auth
{
    public class clsTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public clsTokenResponse(){}
        public clsTokenResponse(string accessToken, string refreshToken) {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }
}
