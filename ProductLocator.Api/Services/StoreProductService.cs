using AutoMapper;
using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreProductService
{
    public readonly AppDbContext _db;

    private readonly IMapper _mapper;

    public StoreProductService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<IEnumerable<StoreProductResponse>>> GetAllStoreProductsAsync(int storeId)
    {
        try
        {
            var storeProducts = await _db.StoreProducts
                .Where(sp => sp.StoreId == storeId)
                .ToListAsync();

            if (!storeProducts.Any())
            {
                return ServiceResponse.Ok(
                    Enumerable.Empty<StoreProductResponse>(),
                     "No store products found");
            }

            var data = _mapper.Map<IEnumerable<StoreProductResponse>>(storeProducts);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<IEnumerable<StoreProductResponse>>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<StoreProductResponse>> GetStoreProductAsync(int storeId, int productId)
    {
        try
        {
            var storeProduct = await _db.StoreProducts.FindAsync(storeId, productId);
            if (storeProduct == null)
            {
                return ServiceResponse.Fail<StoreProductResponse>("Store product not found", 404);
            }

            var data = _mapper.Map<StoreProductResponse>(storeProduct);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<StoreProductResponse>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<StoreProductResponse>> CreateStoreProductAsync(
        int storeId,
        CreateStoreProductRequest req)
    {
        try
        {
            var store = await _db.Stores.FindAsync(storeId);
            if (store == null)
            {
                return ServiceResponse.Fail<StoreProductResponse>("Store not found", 404);
            }

            var product = await _db.Products.FindAsync(req.ProductId);
            if (product == null)
            {
                return ServiceResponse.Fail<StoreProductResponse>("Product not found", 404);
            }

            var existingStoreProduct = await _db.StoreProducts.FindAsync(storeId, req.ProductId);
            if (existingStoreProduct != null)
            {
                return ServiceResponse.Fail<StoreProductResponse>("Store product already exists", 400);
            }

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

            var data = _mapper.Map<StoreProductResponse>(storeProduct);
            return ServiceResponse.Created(data, "Store product created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<StoreProductResponse>(ex.Message, 500);
        }
    }
}