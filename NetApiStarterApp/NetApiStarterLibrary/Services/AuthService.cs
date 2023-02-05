using System;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetApiStarterApp.Data;
using NetApiStarterApp.NetApiStarterLibrary;
using NetApiStarterApp.NetApiStarterLibrary.Models;
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
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtUtils _jwtUtils;
        private ApiUser _user;

        public AuthService(ApiDbContext context,  IConfiguration configuration, UserManager<ApiUser> userManager, RoleManager<ApiRole> roleManager,
            TokenValidationParameters tokenValidationParameters, ILogger<AuthService> logger, JwtUtils jwtUtils)
		{
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
            _jwtUtils = jwtUtils;
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

        public async Task<AuthResponse> CreateJwtAuthResponse(string userName)
        {
            var claims = await GetClaims(userName);
            var rawJwtToken = _jwtUtils.GenerateRawJwtToken(claims);

            var jwtToken = _jwtUtils.FormatTokenToString(rawJwtToken);

            var refreshToken = await _jwtUtils.GenerateAndStoreRefreshToken(rawJwtToken, userName);

            return new AuthResponse
            {
                Success = true,
                UserName = userName,
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };

        }

        public async Task<bool> ValidateUser(LoginApiUserDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);
            var passcheck = await _userManager.CheckPasswordAsync(_user, userDTO.Password);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
        }

        public async Task<AuthResponse> RefreshToken(TokenRequestDTO tokenRequest)
        {
            try
            {
                var authResult = await VerifyRefreshTokenAndGenerateJwt(tokenRequest);

                if (authResult == null)
                {
                    throw new ApiServiceException("Invalid Token");
                }

                return authResult;
            }
            catch (ApiServiceException ex)
            {
                return new AuthResponse()
                {
                    Success = false,
                    Errors = new List<string>() {
                            ex.Message
                    }
                };
            }
            catch (InvalidDataException ex)
            {
                return new AuthResponse()
                {
                    Success = false,
                    Errors = new List<string>() {
                            ex.Message
                    }
                };
            }
            catch (Exception)
            {
                return new AuthResponse()
                {
                    Success = false,
                    Errors = new List<string>() {
                            AuthConstants.UnknownAuthError
                    }
                };
            }
        }

        private async Task<AuthResponse> VerifyRefreshTokenAndGenerateJwt(TokenRequestDTO tokenRequest)
        {
            try
            {

                var validToken = await _jwtUtils.VerifyRefreshTokenAndInvalidate(tokenRequest);
                string userName = String.Empty;

                if (validToken != null)
                {

                    userName = validToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
                    if (userName == null)
                    {
                        throw new InvalidDataException("Invalid UserName");
                    }

                    return await CreateJwtAuthResponse(userName);
                }
                else
                {
                    throw new Exception("Unexpected Error - Token");
                }
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DoesUserExist(string userName)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            return (existingUser != null);
        }

        private async Task<List<Claim>> GetClaims(string userName)
        {
            var claims = _jwtUtils.GetStandardClaims(userName);

            var roles = await GetRolesForUser(userName);
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

        private async Task<IList<string>> GetRolesForUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

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


        

        

  

    }
}

