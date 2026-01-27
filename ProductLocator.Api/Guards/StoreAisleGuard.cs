using ProductLocator.Api.Data;

namespace ProductLocator.Api.Guards;

public sealed class StoreAisleGuard
{
    private readonly AppDbContext _db;

    public StoreAisleGuard(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnsureNameUniqueAsync(int storeId, string aisleName)
    {
        var exists = await _db.StoreAisles
            .AnyAsync(sa => sa.StoreId == storeId && sa.Name == aisleName);
        if (exists)
        {
            throw new ConflictException("Store aisle with the same name already exists in this store");
        }
    }

}