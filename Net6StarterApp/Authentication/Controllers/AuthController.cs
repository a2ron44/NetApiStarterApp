using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Authentication.Services;
using Net6StarterApp.Models;

namespace Net6StarterApp.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
	{
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IMapper mapper)
		{
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            
            try
            {
                //check if user exists
                var doesUserExist = await _authService.DoesUserExist(userDTO.Email);
                if(doesUserExist)
                {
                    _logger.LogError($"User {userDTO.Email} already exists");
                    return BadRequest(new ApiError($"User {userDTO.Email} already exists"));
                }
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = user.Email;
                var result = await _authService.CreateNormalUser(user, userDTO.Password);

                if (!result)
                {
                    return BadRequest(new ApiError(AuthConstants.UknownRegistrationError));
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Register)}");
                return Problem("Unexpected Issue");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");

            try
            {

                if(! await _authService.ValidateUser(userDTO))
                {
                   // return Unauthorized();
                    return BadRequest(new ApiError(AuthConstants.InvalidLogin));
                }

                return Accepted(new { Token = await _authService.CreateToken() });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Login)}");
                return Problem("Unexpected Issue");
            }
        }
    }
}

