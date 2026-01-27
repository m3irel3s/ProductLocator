using ProductLocator.Api.Data;

namespace ProductLocator.Api.Guards;

public sealed class StoreProductGuard
{
    private readonly AppDbContext _db;

    public StoreProductGuard(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnsureExistsAsync(int storeId, int productId)
    {
        var storeProduct = await _db.StoreProducts.FindAsync(storeId, productId);
        if (storeProduct == null)
        {
            throw new NotFoundException("Store product not found");
        }
    }

    public async Task EnsureNotExistsAsync(int storeId, int productId)
    {
        var storeProduct = await _db.StoreProducts.FindAsync(storeId, productId);
        if (storeProduct != null)
        {
            throw new ConflictException("Store product already exists");
        }
    }
}