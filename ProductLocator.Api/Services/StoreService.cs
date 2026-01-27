using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public StoreService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StoreResponse>> GetAllStoresAsync()
    {
        var stores = await _db.Stores.ToListAsync();

        return _mapper.Map<IEnumerable<StoreResponse>>(stores);
    }

    public async Task<StoreResponse> GetStoreByIdAsync(int storeId)
    {
        var store = await _db.Stores.FindAsync(storeId);
        if (store == null)
        {
            throw new NotFoundException("Store not found");
        }

        return _mapper.Map<StoreResponse>(store);
    }

    public async Task<StoreResponse> CreateStoreAsync(CreateStoreRequest req)
    {
        var existingStore = await _db.Stores
            .FirstOrDefaultAsync(s => s.Name == req.Name && s.Location == req.Location);
        if (existingStore != null)
        {
            throw new ConflictException("Store with the same name and location already exists");
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

        return _mapper.Map<StoreResponse>(store);
    }
}