using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductLocator.Api.Data;

namespace ProductLocator.Api.IntegrationTests.Fixtures;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var optionsDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (optionsDescriptor != null)
                services.Remove(optionsDescriptor);

            var contextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(AppDbContext));
            if (contextDescriptor != null)
                services.Remove(contextDescriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_connectionString));
        });
    }
}
