//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication.OAuth;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Net6StarterApp.Auth.Models;
//using Net6StarterApp.Auth.Utilities;
//using Net6StarterApp.Data;

//namespace Net6StarterApp.Auth
//{
//    public class AuthService : IAuthService
//    {
//        private readonly ILogger<AuthService> _logger;
//        private readonly UserManager<ApiUser> _userManager;
//        private readonly JwtUtils _jwtUtils;
//        private readonly IUnitOfWork _uow;

//        public AuthService(ILogger<AuthService> logger, UserManager<ApiUser> userManager, JwtUtils jwtUtils, IUnitOfWork uow)
//        {
//            _logger = logger;
//            _userManager = userManager;
//            _jwtUtils = jwtUtils;
//            _uow = uow;
//        }

//        public async Task<AuthResult> CheckAndAddUser(AddUserRequestDto user)
//        {

//            try
//            {
//                var existingUser = await _userManager.FindByEmailAsync(user.Email.ToLower());

//                if (existingUser != null)
//                {
//                    return new AuthResult()
//                    {
//                        Success = true,
//                        Errors = new List<string>() {
//                            "User already exists"
//                        }
//                    };
//                }

//                var newUser = new ApiUser() { Email = user.Email.ToLower(), UserName = user.Email.ToLower() };
//                var isCreated = await _userManager.CreateAsync(newUser, AuthConstants.DEFAULT_PASSWORD);
//                await _uow.CompleteAsync();

//                if (isCreated.Succeeded)
//                {
//                    return new AuthResult()
//                    {
//                        Success = true
//                    };
//                }
//                else
//                {
//                    _logger.LogWarning("Add User Failed");
//                    throw new Exception();
//                }
//            }
//            catch (Exception)
//            {
//                return new AuthResult()
//                {
//                    Success = false,
//                    Errors = new List<string>() {
//                        AuthConstants.UnknownAuthError
//                    }
//                };
//            }
//        }

//        public async Task<AuthResult> Login(UserLoginRequestDto user)
//        {
//            try
//            {
//                // check if the user with the same email exist
//                var existingUser = await _userManager.FindByEmailAsync(user.Email);

//                if (existingUser == null)
//                {
//                    throw new InvalidDataException(AuthConstants.InvalidLogin);
//                }

//                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

//                if (!isCorrect)
//                {
//                    throw new InvalidDataException(AuthConstants.InvalidLogin);
//                }

//                var jwtToken = _jwtUtils.GenerateAuthServerJwt(existingUser.Email);

//                return jwtToken;

//            }
//            catch (InvalidDataException ex)
//            {
//                return new AuthResult()
//                {
//                    Success = false,
//                    Errors = new List<string>() {
//                            ex.Message
//                    }
//                };
//            }
//            catch (Exception)
//            {
//                return new AuthResult()
//                {
//                    Success = false,
//                    Errors = new List<string>() {
//                            AuthConstants.UnknownAuthError
//                    }
//                };
//            }
//        }


//    }

//}

