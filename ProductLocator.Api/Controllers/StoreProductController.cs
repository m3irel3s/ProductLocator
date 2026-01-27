using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/store/{storeId}/product")]
public class StoreProductController : ControllerBase
{
    private readonly StoreProductService _service;

    public StoreProductController(StoreProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetStoreProducts(int storeId)
    {
        var storeProducts = await _service.GetAllStoreProductsAsync(storeId);
        return Ok(storeProducts);
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetStoreProduct(
        int storeId, int productId)
    {
        var storeProduct = await _service.GetStoreProductAsync(storeId, productId);
        return Ok(storeProduct);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStoreProduct(
        int storeId,
        [FromBody] CreateStoreProductRequest req)
    {
        var storeProduct = await _service.CreateStoreProductAsync(storeId, req);
        return CreatedAtAction(
            nameof(GetStoreProduct),
            new { storeId = storeProduct.StoreId, productId = storeProduct.ProductId },
            storeProduct);
    }
}
