

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
    public async Task Get_all_products_returns_200_with_empty_list()
    {
        var get = await _http.GetAsync("/api/product");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task Get_product_when_missing_returns_404()
    {
        var res = await _http.GetAsync("/api/product/999999");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Get_product_when_exists_returns_200()
    {
        var productId = await _api.CreateProductAsync();

        var get = await _http.GetAsync("/api/product/" + productId);
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task Create_product_returns_201()
    {
        var res = await _http.PostAsJsonAsync("/api/product", new
        {
            name = "Test Product",
            barcode = "1234567890"
        });

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact]
    public async Task Create_product_with_same_barcode_returns_409()
    {
        await _api.CreateProductAsync(name: "Product A", barcode: "123456");
        var sameProduct = await _http.PostAsJsonAsync("/api/product", new
        {
            name = "Product B",
            barcode = "123456"
        });

        Assert.Equal(HttpStatusCode.Conflict, sameProduct.StatusCode);
    }

    [Fact]
    public async Task Create_product_with_wrong_body_returns_400()
    {
        var res = await _http.PostAsJsonAsync("/api/product",
            new { wrong = "data" });

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }
}
