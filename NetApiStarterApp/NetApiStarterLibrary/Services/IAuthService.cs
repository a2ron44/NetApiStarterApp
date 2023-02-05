using NetApiStarterApp.NetApiStarterLibrary.Models;
using System.Security.Claims;
using NetApiStarterLibrary.Models;

namespace NetApiStarterLibrary.Services
{
	public interface IAuthService
	{

		Task<bool> DoesUserExist(string userName);
		Task<bool> CreateNormalUser(ApiUser user, string password);
		Task<AuthResponse> CreateJwtAuthResponse(string userName);
        Task<bool> ValidateUser(LoginApiUserDTO userDTO);
		Task<AuthResponse> RefreshToken(TokenRequestDTO tokenRequest);


    }
}

