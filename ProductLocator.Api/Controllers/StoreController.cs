using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("/api/store/")]
public class StoreController : ControllerBase
{
    private readonly StoreService _service;

    public StoreController(StoreService storeService)
    {
        _service = storeService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<StoreResponse>>>> GetStores()
    {
        var result = await _service.GetAllStoresAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{storeId:int}")]
    public async Task<ActionResult<ServiceResponse<StoreResponse>>> GetStore(int storeId)
    {
        var result = await _service.GetStoreByIdAsync(storeId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<StoreResponse>>> CreateStore(
        CreateStoreRequest req)
    {
        var result = await _service.CreateStoreAsync(req);
        return StatusCode(result.StatusCode, result);
    }
}