using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Authentication.Permissions;

namespace Net6StarterApp.Authentication.Services
{
	public class AuthService : IAuthService
	{
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<AuthService> _logger;
        private ApiUser _user;

        public AuthService(IConfiguration configuration, UserManager<ApiUser> userManager, ILogger<AuthService> logger)
		{
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> CreateUser(ApiUser user, string password)
        {

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Todo show DuplicateUserName,  DuplicateEmail errors
                _logger.LogError($"Error in {nameof(AuthService)}");
                return false;
            }
            return true;

        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUser(LoginUserDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);
            var passcheck = await _userManager.CheckPasswordAsync(_user, userDTO.Password);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
        }

        //public async Task<bool> IsPasswordValid(string password)
        //{
        //    var passcheck = await _userManager.PasswordValidators.ValidateAsync(password);
        //}

        public async Task<bool> DoesUserExist(string userName)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            return (existingUser != null);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtKey = _configuration.GetSection("JWT_KEY").Value;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var permissions = GetPermissionClaims(_user.UserName);
            foreach(var perm in permissions)
            {
                claims.Add(perm);
            }

            return claims;
        }

        private  List<Claim> GetPermissionClaims(string userName)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(AuthConstants.CustomClaimPermissions, Permission.ViewData.ToString()));
            claims.Add(new Claim(AuthConstants.CustomClaimPermissions, Permission.EditData.ToString()));
            return claims;
            
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenExpiryMinutes = Convert.ToDouble(_configuration.GetSection("TokenExpiryMinutes").Value);

            var options = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                audience: jwtSettings.GetSection("Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenExpiryMinutes),
                signingCredentials: signingCredentials
                );

            return options;
        }
    }
}

