using ProductLocator.Api.Data;

namespace ProductLocator.Api.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserGuard _userGuard;
    private readonly PasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext dbContext, IMapper mapper, UserGuard userGuard, PasswordHasher passwordHasher, ITokenService tokenService)
    {
        _db = dbContext;
        _mapper = mapper;
        _userGuard = userGuard;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<UserSummary> RegisterAsync(RegisterRequest request)
    {
        PasswordGuard.EnsureStrong(request.Password);
        await _userGuard.EnsureEmailUniqueAsync(request.Email);
        await _userGuard.EnsureUsernameUniqueAsync(request.Username);

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Role = request.Role,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return _mapper.Map<UserSummary>(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        var (accessToken, refreshToken) = await IssueTokensAsync(user);

        return new LoginResponse(
            _mapper.Map<UserSummary>(user),
            accessToken,
            refreshToken
        );
    }

    public async Task<UserSummary> GetUserByIdAsync(int userId)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        return _mapper.Map<UserSummary>(user);
    }

    public async Task<(string accessToken, string refreshToken)> IssueTokensAsync(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshPlain = RefreshTokenUtils.GeneratePlainToken();
        var refreshHash = RefreshTokenUtils.Hash(refreshPlain);

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();

        return (accessToken, refreshPlain);
    }

    public async Task<RefreshResponse> RefreshAsync(RefreshRequest request)
    {
        var refreshHash = RefreshTokenUtils.Hash(request.RefreshToken);

        var storedToken = await _db.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == refreshHash);

        if (storedToken == null || !storedToken.IsActive)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        var user = storedToken.User;

        var (newAccessToken, newRefreshToken) = await IssueTokensAsync(user);

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.ReplacedByTokenHash = RefreshTokenUtils.Hash(newRefreshToken);

        await _db.SaveChangesAsync();

        return new RefreshResponse(newAccessToken, newRefreshToken);
    }

    public async Task LogoutAsync(LogoutRequest request)
    {
        var refreshHash = RefreshTokenUtils.Hash(request.RefreshToken);

        var storedToken = await _db.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == refreshHash);

        if (storedToken == null || !storedToken.IsActive) return;

        storedToken.RevokedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
