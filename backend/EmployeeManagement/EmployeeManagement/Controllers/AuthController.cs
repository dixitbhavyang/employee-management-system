using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// User login - returns JWT token
        /// </summary>
        /// <param name="request">Login credentials</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for username: {Username}", request.Username);

            var response = await _authService.LoginAsync(request);

            _logger.LogInformation("User {Username} logged in successfully", request.Username);

            return Ok(response);
        }

        /// <summary>
        /// User registration - creates new user and returns JWT token
        /// </summary>
        /// <param name="request">Registration details</param>
        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for username: {Username}", request.Username);

            var response = await _authService.RegisterAsync(request);

            _logger.LogInformation("User {Username} registered successfully", request.Username);

            return CreatedAtAction(nameof(Login), response);
        }

        /// <summary>
        /// Check if username is available
        /// </summary>
        /// <param name="username">Username to check</param>
        [HttpGet("check-username")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckUsername([FromQuery] string username)
        {
            var exists = await _authService.UsernameExistsAsync(username);
            return Ok(exists);
        }
    }
}
