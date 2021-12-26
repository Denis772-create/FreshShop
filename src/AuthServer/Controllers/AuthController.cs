using AuthServer.Filters;
using AuthServer.Interfaces;
using AuthServer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServer.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(IIdentityService identityService, IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _refreshTokenService = refreshTokenService;
        }
        [HttpPost("/login")]
        [Validate]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _identityService.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpPost("/register")]
        [Validate]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpDelete("/logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.User.FindFirstValue("id");

            if (userId is null)
                return Unauthorized();

            await _refreshTokenService.DeleteAll(Guid.Parse(userId));

            return NoContent();
        }

        [HttpPost("refresh")]
        [Validate]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _identityService.RefreshTokenAsync(request);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result);
        }
    }
}
