using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class ProductService
{
    public readonly AppDbContext _db;

    public ProductService(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _db.Products.FindAsync(productId);
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest req)
    {
        var existingProduct = await _db.Products.AnyAsync(x => x.Barcode == req.Barcode);
        if (existingProduct)
            throw new Exception("Product with the same barcode already exists");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Barcode = req.Barcode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return product;
    }
}
