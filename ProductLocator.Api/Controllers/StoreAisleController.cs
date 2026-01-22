using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/store/{storeId:int}/aisle")]
public class StoreAisleController : ControllerBase
{
    private readonly StoreAisleService _service;

    public StoreAisleController(StoreAisleService storeAisleService)
    {
        _service = storeAisleService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<StoreAisleResponse>>>> GetStoreAisles(int storeId)
    {
        var result = await _service.GetAllStoreAislesAsync(storeId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{aisleId:int}")]
    public async Task<ActionResult<ServiceResponse<StoreAisleResponse>>> GetStoreAisle(int storeId, int aisleId)
    {
        var result = await _service.GetStoreAisleAsync(storeId, aisleId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<StoreAisleResponse>>> CreateStoreAisle(int storeId, CreateStoreAisleRequest req)
    {
        var result = await _service.CreateStoreAisleAsync(storeId, req);
        return StatusCode(result.StatusCode, result);
    }
}