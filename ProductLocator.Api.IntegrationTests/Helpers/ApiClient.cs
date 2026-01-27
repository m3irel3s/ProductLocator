using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
namespace ProductLocator.Api.IntegrationTests.Helpers;

public class ApiClient
{
    private readonly HttpClient _http;
    public ApiClient(HttpClient http) => _http = http;

    public async Task<int> CreateStoreAsync(string name, string location)
    {
        var res = await _http.PostAsJsonAsync("/api/store", new { name, location });
        if (res.StatusCode != HttpStatusCode.Created)
            throw new Exception(await res.Content.ReadAsStringAsync());

        using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("data").GetProperty("id").GetInt32();
    }

    public async Task<int> CreateProductAsync(string name, string barcode)
    {
        var res = await _http.PostAsJsonAsync("/api/product", new { name, barcode });
        if (res.StatusCode != HttpStatusCode.Created)
            throw new Exception(await res.Content.ReadAsStringAsync());

        using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("data").GetProperty("id").GetInt32();
    }

    public async Task<int> CreateStoreAisleAsync(int storeId, string name, int maxShelf)
    {
        var res = await _http.PostAsJsonAsync($"/api/store/{storeId}/aisle", new { name, maxShelf });
        if (res.StatusCode != HttpStatusCode.Created && res.StatusCode != HttpStatusCode.OK)
            throw new Exception(await res.Content.ReadAsStringAsync());

        using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("data").GetProperty("id").GetInt32();
    }

    public async Task<HttpResponseMessage> CreateStoreProductAsync(
        int storeId,
        int productId,
        decimal price,
        int? aisleId = null,
        int? shelfNumber = null)
    {
        return await _http.PostAsJsonAsync($"/api/store/{storeId}/product", new
        {
            productId,
            price,
            aisleId,
            shelfNumber
        });
    }
}
