

[Collection("IntegrationTests")]
public class ProductTests : IAsyncLifetime
{
    private readonly HttpClient _http;
    private readonly ApiClient _api;
    private readonly PostgresFixedDbFixture _postgresFixture;

    public ProductTests(PostgresFixedDbFixture postgresFixture)
    {
        _postgresFixture = postgresFixture;

        var factory = new CustomWebApplicationFactory(postgresFixture.ConnectionString);
        _http = factory.CreateClient();
        _api = new ApiClient(_http);
    }

    public async Task InitializeAsync()
    {
        await _postgresFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_product_returns_404_when_missing()
    {
        var res = await _http.GetAsync("/api/product/999999");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Create_and_get_product_works()
    {
        var productId = await _api.CreateProductAsync("Apple", "9876543210123");

        var get = await _http.GetAsync("/api/product/" + productId);
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task Create_product_with_same_barcode_returns_400()
    {
        var productId = await _api.CreateProductAsync("Banana", "1234567890123");

        var sameProduct = await _http.PostAsJsonAsync("/api/product",
            new { name = "Banana 2", barcode = "1234567890123" });

        Assert.Equal(HttpStatusCode.Conflict, sameProduct.StatusCode);
    }
}
