using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using NetApiStarterLibrary;
using Microsoft.EntityFrameworkCore;
using NetApiStarterApp.NetApiStarterLibrary.Models;
using NetApiStarterApp.Data;
using NetApiStarterLibrary.Services;

namespace NetApiStarterApp.NetApiStarterLibrary
{
	public class JwtUtils
	{
        private readonly IConfiguration _configuration;
        private readonly ApiDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<JwtUtils> _logger;

        public JwtUtils(IConfiguration configuration, ApiDbContext context, TokenValidationParameters tokenValidationParameters, ILogger<JwtUtils> logger)
        {
            _configuration = configuration;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
        }

        public List<Claim> GetStandardClaims(string userName)
        {
            var claims = new List<Claim>
                {
                    new Claim("Id", userName),
                    new Claim(JwtRegisteredClaimNames.Name, userName),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            return claims;
        }


        public SecurityToken GenerateRawJwtToken(List<Claim> claims)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var jwtSettings = _configuration.GetSection("NetApiStarterLibaryConfig");
            var tokenExpiryMinutes = Convert.ToDouble(jwtSettings.GetSection("TokenExpiryMinutes").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtSettings.GetSection("Issuer").Value,
                Audience = jwtSettings.GetSection("Audience").Value,
                Expires = DateTime.Now.AddMinutes(tokenExpiryMinutes),
                NotBefore = DateTime.Now,
                SigningCredentials = GetSigningCredentials()
            };

            return jwtTokenHandler.CreateToken(tokenDescriptor);
        }


        public string FormatTokenToString(SecurityToken token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            return jwtTokenHandler.WriteToken(token);
        }

        

        public async Task<RefreshToken> GenerateAndStoreRefreshToken(SecurityToken token, string email)
        {
            var jwtSettings = _configuration.GetSection("NetApiStarterLibaryConfig");

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                Email = email,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings.GetSection("RefreshTokenExpiryDays").Value)),
                IsRevoked = false,
                Token = CommonUtils.RandomString(25) + Guid.NewGuid()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        

        private SigningCredentials GetSigningCredentials()
        {
            var jwtKey = _configuration.GetSection("JWT_KEY").Value;

            if (jwtKey == null)
            {
                jwtKey = AuthConstants.GetDefaultJwtKey();
            }

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<ClaimsPrincipal> VerifyRefreshTokenAndInvalidate(TokenRequestDTO tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            // Validation 1 - Validation JWT token format.  set lifetime expiration to false (bug from github https://github.com/mohamadlawand087/v8-refreshtokenswithJWT/issues/3)
            _tokenValidationParameters.ValidateLifetime = false;
            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
            _tokenValidationParameters.ValidateLifetime = true;

            // Validation 2 - Validate encryption alg
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    throw new InvalidDataException("Invalid Token");
                }
            }

            // Validation 3 - validate expiry date
            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = CommonUtils.UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                throw new InvalidDataException("Token is not expired yet");
            }

            // validation 4 - validate existence of the token
            var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedRefreshToken == null)
            {
                _logger.LogInformation("Refresh failed, could not find refresh token.");
                throw new InvalidDataException("Invalid Token");
            }

            // Validation 5 - validate if used
            if (storedRefreshToken.IsUsed)
            {
                _logger.LogInformation("Refresh failed, refresh token already used.");
                throw new InvalidDataException("Invalid Token");
            }

            // Validation 6 - validate if revoked
            if (storedRefreshToken.IsRevoked)
            {
                _logger.LogInformation("Refresh failed, refresh token Revoked.");
                throw new InvalidDataException("Invalid Token");
            }

            // Validation 7 - validate the id
            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedRefreshToken.JwtId != jti)
            {
                throw new InvalidDataException("Invalid Token");
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            return tokenInVerification;
        }
    }
}

