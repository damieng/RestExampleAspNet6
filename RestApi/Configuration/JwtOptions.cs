namespace RestApi.Configuration
{
    /// <summary>
    /// Configuration options for JWT tokens.
    /// </summary>
    public class JwtOptions
    {
        public const string Jwt = "Jwt";

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpiresMinutes { get; set; }
    }
}
