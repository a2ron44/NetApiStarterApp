using System;
namespace NetApiStarterApp.NetApiStarterLibrary.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string UserName { get; set; }

    }
}

