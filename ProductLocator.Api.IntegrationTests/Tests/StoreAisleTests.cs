

[Collection("IntegrationTests")]
public class StoreAisleTests : IAsyncLifetime
{
    private readonly HttpClient _http;
    private readonly ApiClient _api;
    private readonly PostgresFixedDbFixture _postgresFixture;

    public StoreAisleTests(PostgresFixedDbFixture postgresFixture)
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
    public async Task Get_store_aisles_returns_404_when_store_missing()
    {
        var res = await _http.GetAsync("/api/store/99999/aisle");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Create_store_aisle_returns_404_when_store_missing()
    {
        var res = await _http.PostAsJsonAsync("/api/store/99999/aisle",
            new { name = "Bebidas", maxShelf = 10 });

        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Create_and_get_store_aisle_works()
    {
        var storeId = await _api.CreateStoreAsync("Mercadona", "Porto");

        var StoreAisle = await _http.PostAsJsonAsync($"/api/store/{storeId}/aisle",
            new { name = "Bebidas", maxShelf = 15 });

        Assert.Equal(HttpStatusCode.Created, StoreAisle.StatusCode);

        var get = await _http.GetAsync($"/api/store/{storeId}/aisle");

        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }
}
