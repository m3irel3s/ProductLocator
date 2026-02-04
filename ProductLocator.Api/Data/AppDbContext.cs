namespace ProductLocator.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Store> Stores { get; set; } = null!;
    public DbSet<StoreMember> StoreMembers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<StoreProduct> StoreProducts { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<StoreAisle> StoreAisles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // users
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.PasswordHash).IsRequired();
            e.Property(x => x.Username).IsRequired();
            e.Property(x => x.Role).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();
        });

        // refresh tokens
        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.ToTable("refresh_tokens");
            e.HasKey(x => x.Id);
            e.Property(x => x.UserId).IsRequired();
            e.Property(x => x.TokenHash).IsRequired();
            e.Property(x => x.ExpiresAt).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.RevokedAt).IsRequired(false);
            e.Property(x => x.ReplacedByTokenId).IsRequired(false);

            e.HasOne(x => x.ReplacedByToken)
                .WithMany()
                .HasForeignKey(x => x.ReplacedByTokenId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.UserId);
            e.HasIndex(x => x.TokenHash).IsUnique();
            e.HasIndex(x => x.ReplacedByTokenId).IsUnique();
        });

        // stores
        modelBuilder.Entity<Store>(e =>
        {
            e.ToTable("stores");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.Property(x => x.Location).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();
        });

        // store members
        modelBuilder.Entity<StoreMember>(e =>
        {
            e.ToTable("store_members");
            e.HasKey(x => new { x.StoreId, x.UserId });
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();

            e.HasOne(x => x.Store)
                .WithMany(x => x.StoreMembers)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // products
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("products");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Barcode).IsUnique();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();
        });

        // store products
        modelBuilder.Entity<StoreProduct>(e =>
        {
            e.ToTable("store_products");
            e.HasKey(x => new { x.StoreId, x.ProductId });
            e.Property(x => x.Price).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();
            e.Property(x => x.AisleId).IsRequired(false);
            e.Property(x => x.ShelfNumber).IsRequired(false);

            e.HasOne(x => x.Aisle)
                .WithMany(a => a.StoreProducts)
                .HasForeignKey(x => x.AisleId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(x => x.Store)
                .WithMany(x => x.StoreProducts)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Product)
                .WithMany(x => x.StoreProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // store aisles
        modelBuilder.Entity<StoreAisle>(e =>
        {
            e.ToTable("store_aisles");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.Property(x => x.MaxShelf).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();

            e.HasIndex(x => new { x.StoreId, x.Name }).IsUnique();

            e.HasOne(x => x.Store)
                .WithMany(x => x.Aisles)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

        });

        // audit logs
        modelBuilder.Entity<AuditLog>(e =>
        {
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Action).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();

            e.HasOne(x => x.ActorUser)
                .WithMany()
                .HasForeignKey(x => x.ActorUserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Store)
                .WithMany()
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
