using Microsoft.AspNetCore.Authorization.Infrastructure;
using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;

    public ProductController(ProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var product = await _service.GetProductByIdAsync(productId);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        CreateProductRequest req)
    {
        var product = await _service.CreateProductAsync(req);
        return CreatedAtAction(
            nameof(GetProduct),
            new { productId = product.Id },
            product);
    }
}
