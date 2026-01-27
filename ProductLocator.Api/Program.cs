using ProductLocator.Api.Services;
using ProductLocator.Api.Data;
using ProductLocator.Api.Middleware;
using ProductLocator.Api.Guards;

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

builder.Services.AddScoped<StoreGuard>();
builder.Services.AddScoped<ProductGuard>();
builder.Services.AddScoped<StoreProductGuard>();
builder.Services.AddScoped<StoreAisleGuard>();

builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
