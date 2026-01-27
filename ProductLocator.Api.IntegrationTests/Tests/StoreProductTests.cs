
[Collection("IntegrationTests")]
public class StoreProductFlowTests : IAsyncLifetime
{
    private readonly HttpClient _http;
    private readonly ApiClient _api;
    private readonly PostgresFixedDbFixture _postgresFixture;

    public StoreProductFlowTests(PostgresFixedDbFixture postgresFixture)
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
    public async Task Full_flow_store_product_works()
    {
        var storeId = await _api.CreateStoreAsync("Pingo Doce", "Porto");
        var productId = await _api.CreateProductAsync("Coca Cola", "1234567890123");
        var aisleId = await _api.CreateStoreAisleAsync(storeId, "Bebidas", 10);

        var create = await _api.CreateStoreProductAsync(
            storeId,
            productId,
            price: 2.00m,
            aisleId: aisleId,
            shelfNumber: 2
        );

        Assert.True(create.IsSuccessStatusCode);

        var get = await _http.GetAsync(
            $"/api/store/{storeId}/product/{productId}");

        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }
}
