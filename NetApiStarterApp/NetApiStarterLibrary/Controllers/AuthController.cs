using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetApiStarterApp.NetApiStarterLibrary.Models;
using NetApiStarterLibrary.Models;
using NetApiStarterLibrary.Services;

namespace NetApiStarterLibrary.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
        public async Task<IActionResult> Register([FromBody] ApiUserDTO userDTO)
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
        public async Task<IActionResult> Login([FromBody] LoginApiUserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");

            try
            {

                if(! await _authService.ValidateUser(userDTO))
                {
                   // return Unauthorized();
                    return BadRequest(new ApiError(AuthConstants.InvalidLogin));
                }

                var authReponse = await _authService.CreateJwtAuthResponse(userDTO.Email);

                return Accepted(authReponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(Login)}");
                return Problem("Unexpected Issue");
            }
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
        {

            try
            {
                var authResult = await _authService.RefreshToken(tokenRequest);

                return Ok(authResult);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok(new AuthResponse()
                {
                    Success = false,
                    Errors = new List<string>() {
                                    ex.Message
                                }
                });
            }
        }
    }
}

