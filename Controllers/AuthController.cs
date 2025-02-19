using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            try
            {
                var token = await _authService.RegisterAsync(dto);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto.Email, dto.Password);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
