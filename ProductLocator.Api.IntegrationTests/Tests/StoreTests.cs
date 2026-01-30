
[Collection("IntegrationTests")]
public class StoreTests : IAsyncLifetime
{
    private readonly HttpClient _http;
    private readonly ApiClient _api;
    private readonly PostgresFixedDbFixture _postgresFixture;

    public StoreTests(PostgresFixedDbFixture postgresFixture)
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
    public async Task Get_stores_returns_200_with_empty_list()
    {
        var res = await _http.GetAsync("/api/store");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Get_store_when_missing_returns_404()
    {
        var res = await _http.GetAsync("/api/store/999999");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Create_store_returns_201()
    {
        var res = await _http.PostAsJsonAsync("/api/store",
            new { name = "Test Store", location = "Test Location" });

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact]
    public async Task Create_store_with_missing_fields_returns_400()
    {
        var res = await _http.PostAsJsonAsync("/api/store",
            new { name = "Test Store" });

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Create_store_with_same_name_and_location_returns_409()
    {
        await _api.CreateStoreAsync(name: "Store A", location: "Location A");
        var sameStore = await _http.PostAsJsonAsync("/api/store",
            new { name = "Store A", location = "Location A" });

        Assert.Equal(HttpStatusCode.Conflict, sameStore.StatusCode);
    }
}
