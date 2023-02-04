using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetApiStarterApp.Data;
using NetApiStarterLibrary.Models;
using NetApiStarterLibrary.Permissions;

namespace NetApiStarterLibrary.Services
{
	public class AuthService : IAuthService
	{
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;
        private readonly RoleManager<ApiRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private ApiUser _user;

        public AuthService(ApiDbContext context,  IConfiguration configuration, UserManager<ApiUser> userManager, RoleManager<ApiRole> roleManager, ILogger<AuthService> logger)
		{
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<bool> CreateNormalUser(ApiUser user, string password)
        {

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Todo show DuplicateUserName,  DuplicateEmail errors
                _logger.LogError($"Error in {nameof(AuthService)}");
                return false;
            }
            var roleResult = await _userManager.AddToRoleAsync(user, RoleType.NormalUser);

            return true;

        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUser(LoginApiUserDTO userDTO)
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

            if (jwtKey == null)
            {
                jwtKey = AuthConstants.GetDefaultJwtKey();
            }

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await GetRolesForUser();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var permissions = await GetPermissionClaims(roles);
            foreach(var perm in permissions)
            {
                claims.Add(perm);
            }

            return claims;
        }

        private async Task<IList<string>> GetRolesForUser()
        {
            var roles = await _userManager.GetRolesAsync(_user);

            return roles;

        }

        private async Task<List<Claim>> GetPermissionClaims(IList<string> roles)
        {
            var claims = new List<Claim>();

            // Highly customizable part.  Assuming Roles are tied to Permissions

            //Get Permissions Linked to Role

            var distinctPerms = await _context.PermissionRoles.Include(r => r.Role).Include(x => x.Permission)
                .Where(x => roles.Contains(x.Role.Name)).Select(x => x.PermissionId).Distinct().ToListAsync();

            foreach(var perm in distinctPerms)
            {
                claims.Add(new Claim(AuthConstants.CustomClaimPermissions, perm.ToString()));
            }


            
            return claims;
            
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("NetApiStarterLibaryConfig");
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

