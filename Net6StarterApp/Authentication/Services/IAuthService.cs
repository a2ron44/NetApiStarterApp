using System;
using Net6StarterApp.Authentication.Models;

namespace Net6StarterApp.Authentication.Services
{
	public interface IAuthService
	{

		Task<bool> DoesUserExist(string userName);
		Task<bool> CreateUser(ApiUser user, string password);
        Task<bool> ValidateUser(LoginUserDTO userDTO);
		Task<string> CreateToken();
		
	}
}

