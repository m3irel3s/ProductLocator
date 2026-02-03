using ProductLocator.Api.Services;
using ProductLocator.Api.Data;
using ProductLocator.Api.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<StoreProductService>();
builder.Services.AddScoped<StoreAisleService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<StoreGuard>();
builder.Services.AddScoped<ProductGuard>();
builder.Services.AddScoped<StoreProductGuard>();
builder.Services.AddScoped<StoreAisleGuard>();
builder.Services.AddScoped<UserGuard>();
builder.Services.AddScoped<PasswordGuard>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer", o =>
  {
      o.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
          ),
          ValidateIssuer = false,
          ValidateAudience = false
      };
  });

builder.Services.AddAuthorization();


builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();

app.Run();
