using AutoMapper;
using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class ProductService
{
    public readonly AppDbContext _db;

    public readonly IMapper _mapper;

    public ProductService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<IEnumerable<ProductResponse>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _db.Products.ToListAsync();
            if (!products.Any())
            {
                return ServiceResponse.Ok(
                    Enumerable.Empty<ProductResponse>(),
                     "No products found");
            }

            var data = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<IEnumerable<ProductResponse>>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<ProductResponse>> GetProductByIdAsync(int productId)
    {
        try
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                return ServiceResponse.Fail<ProductResponse>("Product not found", 404);
            }

            var data = _mapper.Map<ProductResponse>(product);
            return ServiceResponse.Ok(data);
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<ProductResponse>(ex.Message, 500);
        }
    }

    public async Task<ServiceResponse<ProductResponse>> CreateProductAsync(CreateProductRequest req)
    {
        try
        {
            var exists = await _db.Products.AnyAsync(p => p.Barcode == req.Barcode);
            if (exists)
            {
                return ServiceResponse.Fail<ProductResponse>("Product with the same barcode already exists", 409);
            }

            var product = new Product
            {
                Name = req.Name,
                Barcode = req.Barcode,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var data = _mapper.Map<ProductResponse>(product);
            return ServiceResponse.Created(data, "Product created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail<ProductResponse>(ex.Message, 500);
        }
    }
}
