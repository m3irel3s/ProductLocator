using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/store/{storeId:int}/aisle")]
public class StoreAisleController : ControllerBase
{
    private readonly StoreAisleService _service;

    public StoreAisleController(StoreAisleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetStoreAisles(int storeId)
    {
        var aisles = await _service.GetStoreAislesAsync(storeId);
        return Ok(aisles);
    }

    [HttpGet("{aisleId:int}")]
    public async Task<IActionResult> GetStoreAisle(int storeId, int aisleId)
    {
        var aisle = await _service.GetStoreAisleAsync(storeId, aisleId);
        return Ok(aisle);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStoreAisle(int storeId, CreateStoreAisleRequest req)
    {
        var aisle = await _service.CreateStoreAisleAsync(storeId, req);
        return CreatedAtAction(
            nameof(GetStoreAisle),
            new { storeId = storeId, aisleId = aisle.Id },
            aisle);
    }
}