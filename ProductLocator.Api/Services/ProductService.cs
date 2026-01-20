using ProductLocator.Api.Domain;
using ProductLocator.Api.Dtos;

namespace ProductLocator.Api.Services;

public class ProductService
{
    private readonly List<Product> _products = new();

    public Product CreateProduct(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        _products.Add(product);
        return product;
    }

    public Product? GetProduct(Guid id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public List<Product> GetAllProducts()
    {
        return _products;
    }
}