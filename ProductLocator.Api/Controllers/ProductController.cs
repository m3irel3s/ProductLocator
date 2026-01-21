using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;

    public ProductController(ProductService productService)
    {
        _service = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<ProductResponse>>>> GetProducts()
    {
        var result = await _service.GetAllProductsAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{productId:int}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> GetProduct(int productId)
    {
        var result = await _service.GetProductByIdAsync(productId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> CreateProduct(
        CreateProductRequest req)
    {
        var result = await _service.CreateProductAsync(req);
        return StatusCode(result.StatusCode, result);
    }
}
