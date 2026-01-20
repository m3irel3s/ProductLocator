using Microsoft.AspNetCore.Mvc;
using ProductLocator.Api.Dtos;
using ProductLocator.Api.Services;

namespace ProductLocator.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public IActionResult CreateProduct(CreateProductRequest request)
    {
        var product = _productService.CreateProduct(request);

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.CreatedAt
        );

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(Guid id)
    {
        var product = _productService.GetProduct(id);
        if (product == null) return NotFound();

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.CreatedAt
        );

        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetAllProdcuts()
    {
        var products = _productService.GetAllProducts();

        var response = products.Select(product => new ProductResponse(
            product.Id,
            product.Name,
            product.CreatedAt
        ));

        return Ok(response);
    }
}
