using ProductLocator.Api.Services;

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

    


    // [HttpPost("refresh")]
    // [HttpPost("logout")]
    // [HttpGet("me")]


}
