using Microsoft.AspNetCore.Authorization;
using ProductLocator.Api.Services;
using ProductLocator.Api.Extensions;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registeredUser = await _service.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), registeredUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var authenticatedUser = await _service.LoginAsync(request);
        return Ok(authenticatedUser);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.GetUserId();
        var user = await _service.GetUserByIdAsync(userId);
        return Ok(user);
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var newTokens = await _service.RefreshAsync(request);
        return Ok(newTokens);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        await _service.LogoutAsync(request);
        return NoContent();
    }
}
