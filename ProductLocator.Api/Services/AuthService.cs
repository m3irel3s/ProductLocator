using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public AuthService(AppDbContext dbContext, IMapper mapper)
    {
        _db = dbContext;
        _mapper = mapper;
    }

}