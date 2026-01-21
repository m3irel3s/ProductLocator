using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/store/{storeId}/product")]
public class StoreProductController : ControllerBase
{
    private readonly StoreProductService _service;

    public StoreProductController(StoreProductService storeProductService)
    {
        _service = storeProductService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<StoreProductResponse>>>> GetStoreProducts(int storeId)
    {
        var result = await _service.GetAllStoreProductsAsync(storeId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{productId:int}")]
    public async Task<ActionResult<ServiceResponse<StoreProductResponse>>> GetStoreProduct(
        int storeId, int productId)
    {
        var result = await _service.GetStoreProductAsync(storeId, productId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<StoreProductResponse>>> CreateStoreProduct(
        int storeId,
        [FromBody] CreateStoreProductRequest req)
    {
        var result = await _service.CreateStoreProductAsync(storeId, req);
        return StatusCode(result.StatusCode, result);
    }
}
