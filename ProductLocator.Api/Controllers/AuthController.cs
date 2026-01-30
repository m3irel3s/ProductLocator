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

    // [HttpPost("register")]
    // public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    // {
    //     var result = await _service.RegisterAsync(request);
    //     return Ok(result);
    // }



}
