using NetApiStarterLibrary.Models;

namespace NetApiStarterLibrary.Services
{
	public interface IAuthService
	{

		Task<bool> DoesUserExist(string userName);
		Task<bool> CreateNormalUser(ApiUser user, string password);
        Task<bool> ValidateUser(LoginUserDTO userDTO);
		Task<string> CreateToken();
		
	}
}

