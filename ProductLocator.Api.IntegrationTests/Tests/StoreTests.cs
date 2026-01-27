
[Collection("IntegrationTests")]
public class StoreTests : IAsyncLifetime
{
    private readonly HttpClient _http;
    private readonly PostgresFixedDbFixture _postgresFixture;

    public StoreTests(PostgresFixedDbFixture postgresFixture)
    {
        _postgresFixture = postgresFixture;

        var factory = new CustomWebApplicationFactory(postgresFixture.ConnectionString);
        _http = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _postgresFixture.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Create_store_returns_201()
    {
        var res = await _http.PostAsJsonAsync("/api/store", new
        {
            name = "Continente",
            location = "Lisboa"
        });

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }
}
