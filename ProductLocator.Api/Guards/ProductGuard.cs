using ProductLocator.Api.Data;

namespace ProductLocator.Api.Guards;

public sealed class ProductGuard
{
    private readonly AppDbContext _db;

    public ProductGuard(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnsureExistsAsync(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
    }
}