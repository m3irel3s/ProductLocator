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

    public async Task EnsureBarcodeUniqueAsync(string barcode)
    {
        var exists = await _db.Products.AnyAsync(p => p.Barcode == barcode);
        if (exists)
        {
            throw new ConflictException("Product with the same barcode already exists");
        }
    }
}