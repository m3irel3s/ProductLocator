using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreAisleService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public StoreAisleService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

    private async Task EnsureStoreExistsAsync(int storeId)
    {
        var store = await _db.Stores.FindAsync(storeId);
        if (store == null)
        {
            throw new NotFoundException("Store not found");
        }
    }

    public async Task<IEnumerable<StoreAisleResponse>> GetStoreAislesAsync(int storeId)
    {
        await EnsureStoreExistsAsync(storeId);

        var storeAisles = await _db.StoreAisles
            .Where(sa => sa.StoreId == storeId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<StoreAisleResponse>>(storeAisles);
    }

    public async Task<StoreAisleResponse> GetStoreAisleAsync(int storeId, int aisleId)
    {
        await EnsureStoreExistsAsync(storeId);

        var storeAisle = await _db.StoreAisles
            .Include(sa => sa.StoreProducts)
            .FirstOrDefaultAsync(sa => sa.StoreId == storeId && sa.Id == aisleId);

        if (storeAisle == null)
        {
            throw new NotFoundException("Store aisle not found");
        }

        return _mapper.Map<StoreAisleResponse>(storeAisle);
    }

    public async Task<StoreAisleResponse> CreateStoreAisleAsync(
        int storeId,
        CreateStoreAisleRequest req)
    {
        await EnsureStoreExistsAsync(storeId);

        var existingStoreAisle = await _db.StoreAisles
            .FirstOrDefaultAsync(sa => sa.StoreId == storeId && sa.Name == req.Name);
        if (existingStoreAisle != null)
        {
            throw new ConflictException("Store aisle with the same name already exists");
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

        return _mapper.Map<StoreAisleResponse>(storeAisle);
    }
}
