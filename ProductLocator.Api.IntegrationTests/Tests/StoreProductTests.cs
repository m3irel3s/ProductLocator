
using System.Reflection;

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
    public async Task Get_store_products_returns_200_with_empty_list()
    {
        var storeId = await _api.CreateStoreAsync();

        var get = await _http.GetAsync(
            $"/api/store/{storeId}/product");

        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task Create_and_get_store_product_returns_201_and_200()
    {
        var storeId = await _api.CreateStoreAsync();
        var productId = await _api.CreateProductAsync();
        var aisleId = await _api.CreateStoreAisleAsync(storeId);

        var create = await _api.CreateStoreProductAsync(
            storeId,
            productId,
            price: 2.00m,
            aisleId: aisleId,
            shelfNumber: 2
        );

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var get = await _http.GetAsync(
            $"/api/store/{storeId}/product/{productId}");

        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task Get_store_product_when_not_exists_returns_404()
    {
        var storeId = await _api.CreateStoreAsync();
        var productId = await _api.CreateProductAsync();

        var get = await _http.GetAsync(
            $"/api/store/{storeId}/product/{productId}");

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task Get_store_products_when_store_doesnt_exist_returns_404()
    {
        var get = await _http.GetAsync(
            $"/api/store/9999/products");

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task Get_store_product_when_product_doesnt_exist_returns_404()
    {
        var storeId = await _api.CreateStoreAsync();

        var get = await _http.GetAsync(
            $"/api/store/{storeId}/product/9999");

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task Create_store_product_with_nonexistent_store_returns_404()
    {
        var productId = await _api.CreateProductAsync();

        var create = await _api.CreateStoreProductAsync(
            storeId: 9999,
            productId: productId,
            price: 1.50m,
            aisleId: 1,
            shelfNumber: 1
        );

        Assert.Equal(HttpStatusCode.NotFound, create.StatusCode);
    }

    [Fact]
    public async Task Create_store_product_with_nonexistent_product_returns_404()
    {
        var storeId = await _api.CreateStoreAsync();
        var aisleId = await _api.CreateStoreAisleAsync(storeId);
        var productId = 9999;

        var create = await _api.CreateStoreProductAsync(
            storeId,
            productId,
            price: 1.50m,
            aisleId: aisleId,
            shelfNumber: 1
        );

        Assert.Equal(HttpStatusCode.NotFound, create.StatusCode);
    }

    [Fact]
    public async Task Create_store_product_with_nonexistent_aisle_returns_404()
    {
        var storeId = await _api.CreateStoreAsync();
        var productId = await _api.CreateProductAsync();
        var aisleId = 9999;

        var create = await _api.CreateStoreProductAsync(
            storeId,
            productId,
            price: 1.50m,
            aisleId: aisleId,
            shelfNumber: 1
        );

        Assert.Equal(HttpStatusCode.NotFound, create.StatusCode);
    }

    [Fact]
    public async Task Create_store_product_when_already_exists_returns_409()
    {
        var storeId = await _api.CreateStoreAsync();
        var productId = await _api.CreateProductAsync();
        var aisleId = await _api.CreateStoreAisleAsync(storeId);

        var firstCreate = await _api.CreateStoreProductAsync(
            storeId,
            productId,
            price: 2.00m,
            aisleId: aisleId,
            shelfNumber: 2
        );

        Assert.Equal(HttpStatusCode.Created, firstCreate.StatusCode);

        var secondCreate = await _api.CreateStoreProductAsync(
            storeId,
            productId,
            price: 2.00m,
            aisleId: aisleId,
            shelfNumber: 2
        );

        Assert.Equal(HttpStatusCode.Conflict, secondCreate.StatusCode);
    }

}
