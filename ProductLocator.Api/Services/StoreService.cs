using AutoMapper;
using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreService
{
    public readonly AppDbContext _db;

    private readonly IMapper _mapper;

    public StoreService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<IEnumerable<StoreResponse>>> GetAllStoresAsync()
    {
        try
        {
            var stores = await _db.Stores.ToListAsync();

            if (!stores.Any())
            {
                return ServiceResponse.Ok(
                    Enumerable.Empty<StoreResponse>(),
                     "No stores found");
            }

            var data = _mapper.Map<IEnumerable<StoreResponse>>(stores);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<IEnumerable<StoreResponse>>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<StoreResponse>> GetStoreByIdAsync(int storeId)
    {
        try
        {
            var store = await _db.Stores.FindAsync(storeId);
            if (store == null)
            {
                return ServiceResponse.Fail<StoreResponse>("Store not found", 404);
            }

            var data = _mapper.Map<StoreResponse>(store);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<StoreResponse>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<StoreResponse>> CreateStoreAsync(CreateStoreRequest req)
    {
        try
        {
            var existingStore = await _db.Stores
                .FirstOrDefaultAsync(s => s.Name == req.Name && s.Location == req.Location);
            if (existingStore != null)
            {
                return ServiceResponse.Fail<StoreResponse>("Store with the same name and location already exists",400);
            }

            var store = new Store
            {
                Name = req.Name,
                Location = req.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Stores.Add(store);
            await _db.SaveChangesAsync();

            var data = _mapper.Map<StoreResponse>(store);
            return ServiceResponse.Created(data, "Store created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<StoreResponse>(ex.Message, 500);
        }
    }
}