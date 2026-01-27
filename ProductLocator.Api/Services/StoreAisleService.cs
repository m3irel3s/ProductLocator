using AutoMapper;
using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreAisleService
{
    public readonly AppDbContext _db;

    private readonly IMapper _mapper;

    public StoreAisleService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<IEnumerable<StoreAisleResponse>>> GetAllStoreAislesAsync(int storeId)
    {
        try
        {
            var storeAisles = await _db.StoreAisles
                .Where(sa => sa.StoreId == storeId)
                .ToListAsync();

            if (!storeAisles.Any())
            {
                return ServiceResponse.Ok(
                    Enumerable.Empty<StoreAisleResponse>(),
                     "No store aisles found");
            }

            var data = _mapper.Map<IEnumerable<StoreAisleResponse>>(storeAisles);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<IEnumerable<StoreAisleResponse>>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<StoreAisleResponse>> GetStoreAisleAsync(int storeId, int aisleId)
    {
        try
        {
            var storeAisle = await _db.StoreAisles
                .Include(sa => sa.StoreProducts)
                .FirstOrDefaultAsync(sa => sa.StoreId == storeId && sa.Id == aisleId);

            if (storeAisle == null)
            {
                return ServiceResponse.Fail<StoreAisleResponse>("Store aisle not found", 404);
            }

            var data = _mapper.Map<StoreAisleResponse>(storeAisle);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<StoreAisleResponse>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<StoreAisleResponse>> CreateStoreAisleAsync(
        int storeId,
        CreateStoreAisleRequest req)
    {
        try
        {
            var store = await _db.Stores.FindAsync(storeId);
            if (store == null)
            {
                return ServiceResponse.Fail<StoreAisleResponse>("Store not found", 404);
            }

            var existingStoreAisle = await _db.StoreAisles
                .FirstOrDefaultAsync(sa => sa.StoreId == storeId && sa.Name == req.Name);
            if (existingStoreAisle != null)
            {
                return ServiceResponse.Fail<StoreAisleResponse>("Store aisle with the same name already exists", 400);
            }

            var storeAisle = new StoreAisle
            {
                StoreId = storeId,
                Name = req.Name,
                MaxShelf = req.MaxShelf,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _db.StoreAisles.Add(storeAisle);
            await _db.SaveChangesAsync();

            var data = _mapper.Map<StoreAisleResponse>(storeAisle);
            return ServiceResponse.Created(data, "Store aisle created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<StoreAisleResponse>(ex.Message, 500);
        }
    }
}
