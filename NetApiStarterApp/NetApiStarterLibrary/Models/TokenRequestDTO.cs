using System;
using System.ComponentModel.DataAnnotations;

namespace NetApiStarterApp.NetApiStarterLibrary.Models
{
    public class TokenRequestDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }

    }
}

