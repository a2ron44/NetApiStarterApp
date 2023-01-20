//using System;
//using Microsoft.Extensions.Options;
//using Net6StarterApp.Auth.Models;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using Net6StarterApp.Data;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.EntityFrameworkCore;

//namespace Net6StarterApp.Auth.Utilities
//{
//	public class JwtUtils
//	{
//        private readonly ILogger<JwtUtils> _logger;
//        private readonly TokenValidationParameters _tokenValidationParameters;
//        private readonly ApiDbContext _dbContext;
//        private readonly JwtConfig _jwtConfig;

//        public JwtUtils(ILogger<JwtUtils> logger, TokenValidationParameters tokenValidationParameters, ApiDbContext dbContext, IOptionsMonitor<JwtConfig> optionsMonitor)
//        {
//            _logger = logger;
//            _tokenValidationParameters = tokenValidationParameters;
//            _dbContext = dbContext;
//            _jwtConfig = optionsMonitor.CurrentValue;
//        }

//        public AuthResult GenerateAuthServerJwt(string email)
//        {

//            var jwtTokenHandler = new JwtSecurityTokenHandler();

//            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

//            List<Claim> claims = GetStandardClaims(email);

//            var rawJwtToken = GenerateRawJwtToken(claims);

//            var jwtToken = FormatTokenToString(rawJwtToken);

//            return new AuthResult()
//            {
//                Token = jwtToken,
//                Success = true,
//                RefreshToken = null,
//                Email = email
//            };
//        }

//        public SecurityToken GenerateRawJwtToken(List<Claim> claims)
//        {
//            var jwtTokenHandler = new JwtSecurityTokenHandler();

//            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Issuer = _jwtConfig.Issuer,
//                Audience = _jwtConfig.Audience,
//                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenExpiryMinutes),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };

//            return jwtTokenHandler.CreateToken(tokenDescriptor);
//        }

//        public string FormatTokenToString(SecurityToken token)
//        {
//            var jwtTokenHandler = new JwtSecurityTokenHandler();

//            return jwtTokenHandler.WriteToken(token);
//        }



//        //public bool VerifyToken(string token)
//        //{
//        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
//        //    // Validation 1 - Validation JWT token format.  set lifetime expiration to false (bug from github https://github.com/mohamadlawand087/v8-refreshtokenswithJWT/issues/3)
//        //    _tokenValidationParameters.ValidateLifetime = false;
//        //    var tokenInVerification = jwtTokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
//        //    _tokenValidationParameters.ValidateLifetime = true;

//        //    // Validation 2 - Validate encryption alg
//        //    if (validatedToken is JwtSecurityToken jwtSecurityToken)
//        //    {
//        //        var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

//        //        if (result == false)
//        //        {
//        //            throw new InvalidDataException("Invalid Token");
//        //        }
//        //    }

//        //    // Validation 3 - validate expiry date
//        //    var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

//        //    var expiryDate = CommonUtils.UnixTimeStampToDateTime(utcExpiryDate);

//        //    if (expiryDate <= DateTime.UtcNow)
//        //    {
//        //        throw new InvalidDataException("Token Expired, Login and try again");
//        //    }

//        //    return true;
//        //}

//        public async Task<ClaimsPrincipal> VerifyRefreshTokenAndInvalidate(TokenRequest tokenRequest)
//        {
//            var jwtTokenHandler = new JwtSecurityTokenHandler();
//            // Validation 1 - Validation JWT token format.  set lifetime expiration to false (bug from github https://github.com/mohamadlawand087/v8-refreshtokenswithJWT/issues/3)
//            _tokenValidationParameters.ValidateLifetime = false;
//            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
//            _tokenValidationParameters.ValidateLifetime = true;

//            // Validation 2 - Validate encryption alg
//            if (validatedToken is JwtSecurityToken jwtSecurityToken)
//            {
//                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

//                if (result == false)
//                {
//                    throw new InvalidDataException("Invalid Token");
//                }
//            }

//            // Validation 3 - validate expiry date
//            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

//            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

//            if (expiryDate > DateTime.UtcNow)
//            {
//                throw new InvalidDataException("Token is not expired yet");
//            }

//            // validation 4 - validate existence of the token
//            var storedRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

//            if (storedRefreshToken == null)
//            {
//                _logger.LogInformation("Refresh failed, could not find refresh token.");
//                throw new InvalidDataException("Invalid Token");
//            }

//            // Validation 5 - validate if used
//            if (storedRefreshToken.IsUsed)
//            {
//                _logger.LogInformation("Refresh failed, refresh token already used.");
//                throw new InvalidDataException("Invalid Token");
//            }

//            // Validation 6 - validate if revoked
//            if (storedRefreshToken.IsRevoked)
//            {
//                _logger.LogInformation("Refresh failed, refresh token Revoked.");
//                throw new InvalidDataException("Invalid Token");
//            }

//            // Validation 7 - validate the id
//            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

//            if (storedRefreshToken.JwtId != jti)
//            {
//                throw new InvalidDataException("Invalid Token");
//            }

//            storedRefreshToken.IsUsed = true;
//            _dbContext.RefreshTokens.Update(storedRefreshToken);
//            await _dbContext.SaveChangesAsync();

//            return tokenInVerification;
//        }

//        public List<Claim> GetStandardClaims(string email)
//        {
//            var claims = new List<Claim>
//                {
//                    new Claim("Id", email),
//                    new Claim(JwtRegisteredClaimNames.Email, email),
//                    new Claim(JwtRegisteredClaimNames.Sub, email),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//            return claims;
//        }

//        public async Task<RefreshToken> GenerateAndStoreRefreshToken(SecurityToken token, string email)
//        {
//            var refreshToken = new RefreshToken()
//            {
//                JwtId = token.Id,
//                IsUsed = false,
//                Email = email,
//                AddedDate = DateTime.UtcNow,
//                ExpiryDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays),
//                IsRevoked = false,
//                Token = RandomString(25) + Guid.NewGuid()
//            };

//            await _dbContext.RefreshTokens.AddAsync(refreshToken);
//            await _dbContext.SaveChangesAsync();

//            return refreshToken;
//        }


//        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
//        {
//            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
//            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

//            return dateTimeVal;
//        }

//        public static string RandomString(int length)
//        {
//            var random = new Random();
//            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            return new string(Enumerable.Repeat(chars, length)
//            .Select(s => s[random.Next(s.Length)]).ToArray());
//        }
//    }
    
//}

