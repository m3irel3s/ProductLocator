using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;

    public ProductController(ProductService productService)
    {
        _service = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetAllProductsAsync();
        if (products == null) return NotFound();

        var response = products.Select(product => new ProductResponse(
            product.Id,
            product.Name,
            product.Barcode,
            product.CreatedAt,
            product.UpdatedAt
        ));

        return Ok(response);
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var product = await _service.GetProductByIdAsync(productId);
        if (product == null) return NotFound();

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Barcode,
            product.CreatedAt,
            product.UpdatedAt
        );

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> CreateProduct(
        CreateProductRequest req)
    {
        var product = await _service.CreateProductAsync(req);

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Barcode,
            product.CreatedAt,
            product.UpdatedAt
        );

        return CreatedAtAction(
            nameof(GetProduct),
            new { productId = product.Id },
            response
        );
    }

}