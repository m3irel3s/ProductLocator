using ProductLocator.Api.Data;

namespace ProductLocator.Api.Guards;

public sealed class StoreGuard
{
    private readonly AppDbContext _db;

    public StoreGuard(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnsureExistsAsync(int storeId)
    {
        var store = await _db.Stores.FindAsync(storeId);
        if (store == null)
        {
            throw new NotFoundException("Store not found");
        }
    }
}