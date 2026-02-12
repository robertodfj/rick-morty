using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RickYMorty.dto.auth;
using RickYMorty.service.auth;

namespace RickYMorty.controller
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            _logger.LogInformation("Received registration request for user: {Username}", registerDTO.Username);

            await authService.Register(registerDTO);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO registerDTO)
        {
            _logger.LogInformation("Received registration request for admin: {Username}", registerDTO.Username);

            await authService.RegisterAdmin(registerDTO);

            return Ok(new { message = "Admin registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Received login request for user: {Username}", loginDTO.Username);

            var token = await authService.Login(loginDTO);

            return Ok(new { token });
        }
    }
}