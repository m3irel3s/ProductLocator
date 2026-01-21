using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/stores/{storeId}/products")]
public class StoreProductController : ControllerBase
{
    private readonly StoreProductService _service;

    public StoreProductController(StoreProductService storeProductService)
    {
        _service = storeProductService;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetStoreProducts(Guid storeId)
    // {
    //     var products = await _service.GetStoreProductsByStoreIdAsync(storeId);
    //     if (products == null) return NotFound();

    //     var response = products.Select(product => new StoreProductResponse(
    //         product.StoreId,
    //         product.StoreName,
    //         product.ProductId,
    //         product.ProductName,
    //         product.Price,
    //         product.Aisle,
    //         product.Shelf,
    //         product.UpdatedAt
    //     ));

    //     return Ok(response);
    // }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetStoreProduct(Guid storeId, Guid productId)
    {
        var storeProduct = await _service.GetStoreProductAsync(storeId, productId);
        if (storeProduct == null) return NotFound();

        var response = new StoreProductResponse(
            storeProduct.StoreId,
            storeProduct.ProductId,
            storeProduct.Store!.Name,
            storeProduct.Product!.Name,
            storeProduct.Product!.Barcode,
            storeProduct.Price,
            storeProduct.Aisle,
            storeProduct.Shelf,
            storeProduct.CreatedAt,
            storeProduct.UpdatedAt
        );

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<StoreProductResponse>> CreateStoreProduct(
        Guid storeId,
        [FromBody] CreateStoreProductRequest req)
    {
        var storeProduct = await _service.CreateStoreProductAsync(storeId, req);
        if (storeProduct == null) return BadRequest();

        var response = new StoreProductResponse(
            storeProduct.StoreId,
            storeProduct.ProductId,
            storeProduct.Store!.Name,
            storeProduct.Product!.Name,
            storeProduct.Product!.Barcode,
            storeProduct.Price,
            storeProduct.Aisle,
            storeProduct.Shelf,
            storeProduct.CreatedAt,
            storeProduct.UpdatedAt
        );

        return CreatedAtAction(
            nameof(GetStoreProduct),
            new { storeId = response.StoreId, productId = response.ProductId },
            response
        );
    }
}
