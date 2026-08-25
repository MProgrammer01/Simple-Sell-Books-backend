namespace SimpleSellBooks_API.DTOs.Auth
{
    public class clsRefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
