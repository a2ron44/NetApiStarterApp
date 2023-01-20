using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Models;

namespace Net6StarterApp.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
	{
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(UserManager<ApiUser> userManager, ILogger<AuthController> logger, IMapper mapper)
		{
            _userManager = userManager;
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
                var existingUser = await _userManager.FindByNameAsync(userDTO.Email);
                if(existingUser != null)
                {
                    _logger.LogError($"User {existingUser.UserName} already exists");
                    return BadRequest(new ApiError($"User {existingUser.UserName} already exists"));
                }
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    // Todo show DuplicateUserName,  DuplicateEmail errors
                    _logger.LogError($"Error in {nameof(Register)}");
                    return BadRequest(AuthConstants.UknownRegistrationError);
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Register)}");
                return Problem(AuthConstants.UknownRegistrationError);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(AuthConstants.UknownRegistrationError);
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Register)}");
                return Problem(AuthConstants.UknownRegistrationError);
            }
        }
    }
}

