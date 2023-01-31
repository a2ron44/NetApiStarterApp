using System;
using Net6StarterApp.Authentication.Models;

namespace Net6StarterApp.Authentication.Services
{
	public interface IAuthService
	{

		Task<bool> DoesUserExist(string userName);
		Task<bool> CreateNormalUser(ApiUser user, string password);
        Task<bool> ValidateUser(LoginUserDTO userDTO);
		Task<string> CreateToken();
		
	}
}

