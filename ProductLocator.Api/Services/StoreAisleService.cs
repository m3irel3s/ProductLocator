using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class StoreAisleService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly StoreGuard _storeGuard;
    private readonly StoreAisleGuard _storeAisleGuard;

    public StoreAisleService(AppDbContext dbContext, IMapper mapper, StoreGuard storeGuard, StoreAisleGuard storeAisleGuard)
    {
        _db = dbContext;
        _mapper = mapper;
        _storeGuard = storeGuard;
        _storeAisleGuard = storeAisleGuard;
    }

    public async Task<IEnumerable<StoreAisleResponse>> GetStoreAislesAsync(int storeId)
    {
        await _storeGuard.EnsureExistsAsync(storeId);

        var storeAisles = await _db.StoreAisles
            .Where(sa => sa.StoreId == storeId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<StoreAisleResponse>>(storeAisles);
    }

    public async Task<StoreAisleResponse> GetStoreAisleAsync(int storeId, int aisleId)
    {
        await _storeGuard.EnsureExistsAsync(storeId);

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
        await _storeGuard.EnsureExistsAsync(storeId);
        await _storeAisleGuard.EnsureNameUniqueAsync(storeId, req.Name);

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
