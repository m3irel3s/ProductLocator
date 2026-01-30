using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreProductService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly StoreGuard _storeGuard;
    private readonly ProductGuard _productGuard;
    private readonly StoreProductGuard _storeProductGuard;
    private readonly StoreAisleGuard _storeAisleGuard;

    public StoreProductService(AppDbContext db, IMapper mapper, StoreGuard storeGuard, ProductGuard productGuard, StoreProductGuard storeProductGuard, StoreAisleGuard storeAisleGuard)
    {
        _db = db;
        _mapper = mapper;
        _storeGuard = storeGuard;
        _productGuard = productGuard;
        _storeProductGuard = storeProductGuard;
        _storeAisleGuard = storeAisleGuard;
    }

    public async Task<IEnumerable<StoreProductResponse>> GetAllStoreProductsAsync(int storeId)
    {
        await _storeGuard.EnsureExistsAsync(storeId);

        var storeProducts = await _db.StoreProducts
            .Where(sp => sp.StoreId == storeId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<StoreProductResponse>>(storeProducts);
    }

    public async Task<StoreProductResponse> GetStoreProductAsync(int storeId, int productId)
    {
        await _storeGuard.EnsureExistsAsync(storeId);
        await _productGuard.EnsureExistsAsync(productId);

        var storeProduct = await _db.StoreProducts.FindAsync(storeId, productId);
        if (storeProduct == null)
        {
            throw new NotFoundException("Store product not found");
        }

        return _mapper.Map<StoreProductResponse>(storeProduct);
    }

    public async Task<StoreProductResponse> CreateStoreProductAsync(
        int storeId,
        CreateStoreProductRequest req)
    {
        await _storeGuard.EnsureExistsAsync(storeId);
        await _productGuard.EnsureExistsAsync(req.ProductId);
        if (req.AisleId.HasValue) await _storeAisleGuard.EnsureExistsAsync(req.AisleId.Value);
        await _storeProductGuard.EnsureNotExistsAsync(storeId, req.ProductId);

        var storeProduct = new StoreProduct
        {
            StoreId = storeId,
            ProductId = req.ProductId,
            Price = req.Price,
            AisleId = req.AisleId,
            ShelfNumber = req.ShelfNumber,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.StoreProducts.Add(storeProduct);
        await _db.SaveChangesAsync();

        return _mapper.Map<StoreProductResponse>(storeProduct);
    }
}