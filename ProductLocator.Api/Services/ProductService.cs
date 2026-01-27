using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class ProductService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly ProductGuard _productGuard;

    public ProductService(AppDbContext dbContext, IMapper mapper, ProductGuard productGuard)
    {
        _db = dbContext;
        _mapper = mapper;
        _productGuard = productGuard;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        var products = await _db.Products.ToListAsync();

        return _mapper.Map<IEnumerable<ProductResponse>>(products);
    }

    public async Task<ProductResponse> GetProductByIdAsync(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }

        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest req)
    {
        await _productGuard.EnsureBarcodeUniqueAsync(req.Barcode);

        var product = new Product
        {
            Name = req.Name,
            Barcode = req.Barcode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return _mapper.Map<ProductResponse>(product);
    }
}
