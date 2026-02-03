using ProductLocator.Api.Data;

namespace ProductLocator.Api.Guards;

public sealed class UserGuard
{
    private readonly AppDbContext _db;

    public UserGuard(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnsureEmailUniqueAsync(string email)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == email);
        if (exists)
        {
            throw new ConflictException("User with the same email already exists");
        }
    }

    public async Task EnsureUsernameUniqueAsync(string username)
    {
        var exists = await _db.Users.AnyAsync(u => u.Username == username);
        if (exists)
        {
            throw new ConflictException("User with the same username already exists");
        }
    }
}