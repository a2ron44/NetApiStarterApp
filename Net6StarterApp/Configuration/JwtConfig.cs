using System;
namespace Net6StarterApp.Configuration
{

    public class JwtConfig
    {
        public string Secret { get; set; } = String.Empty;

        public int TokenExpiryMinutes { get; set; }

        public int RefreshTokenExpiryDays { get; set; }

        public string Issuer { get; set; } = String.Empty;

        public string Audience { get; set; } = String.Empty;
    }
    
}

