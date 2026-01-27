using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("/api/store/")]
public class StoreController : ControllerBase
{
    private readonly StoreService _service;

    public StoreController(StoreService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetStores()
    {
        var stores = await _service.GetAllStoresAsync();
        return Ok(stores);
    }

    [HttpGet("{storeId:int}")]
    public async Task<IActionResult> GetStore(int storeId)
    {
        var store = await _service.GetStoreByIdAsync(storeId);
        return Ok(store);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStore(
        CreateStoreRequest req)
    {
        var store = await _service.CreateStoreAsync(req);
        return CreatedAtAction(
            nameof(GetStore),
            new { storeId = store.Id },
            store);
    }
}