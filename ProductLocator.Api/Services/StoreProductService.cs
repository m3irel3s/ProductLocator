using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreProductService
{
    public readonly AppDbContext _db;

    public StoreProductService(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<StoreProduct?> GetStoreProductAsync(Guid storeId, Guid productId)
    {
        return await _db.StoreProducts
            .Include(x => x.Store)
            .Include(x => x.Product)
            .FirstOrDefaultAsync(sp => sp.StoreId == storeId && sp.ProductId == productId);
    }


    public async Task<StoreProduct> CreateStoreProductAsync(
        Guid storeId,
        CreateStoreProductRequest req)
    {
        // var store = await _db.Stores.FindAsync(storeId);
        // if (store == null)
        //     throw new Exception("Store not found");

        var product = await _db.Products.FindAsync(req.ProductId);
        if (product == null)
            throw new Exception("Product not found");

        var existingStoreProduct = await _db.StoreProducts.FindAsync(storeId, req.ProductId);
        if (existingStoreProduct != null)
            throw new Exception("Product already exists in store");

        var storeProduct = new StoreProduct
        {
            StoreId = storeId,
            ProductId = req.ProductId,
            Price = req.Price,
            Aisle = req.Aisle,
            Shelf = req.Shelf,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.StoreProducts.Add(storeProduct);
        await _db.SaveChangesAsync();

        return await _db.StoreProducts
            .Include(x => x.Store)
            .Include(x => x.Product)
            .FirstAsync(x => x.StoreId == storeId && x.ProductId == req.ProductId);
    }
}