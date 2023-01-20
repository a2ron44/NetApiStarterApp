//using System;
//using System.ComponentModel.DataAnnotations;

//namespace Net6StarterApp.Auth.Models
//{
//    public class RefreshToken
//    {
//        public int Id { get; set; }
//        [MaxLength(30)]
//        public string Email { get; set; }
//        public string Token { get; set; }
//        public string JwtId { get; set; } // Map the token with jwtId
//        public bool IsUsed { get; set; } // if its used we dont want generate a new Jwt token with the same refresh token
//        public bool IsRevoked { get; set; } // if it has been revoke for security reasons
//        public DateTime AddedDate { get; set; }
//        public DateTime ExpiryDate { get; set; } // Refresh token is long lived it could last for months.

//    }
//}

