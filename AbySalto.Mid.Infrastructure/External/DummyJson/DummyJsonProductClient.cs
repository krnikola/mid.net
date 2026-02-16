using System.Net.Http.Json;

namespace AbySalto.Mid.Infrastructure.External.DummyJson;

public class DummyJsonProductClient
{
    private readonly HttpClient _http;

    public DummyJsonProductClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ProductsResponse?> GetProductsAsync(int skip = 0, int limit = 30)
    {
        // DummyJSON podržava skip/limit
        return await _http.GetFromJsonAsync<ProductsResponse>($"products?skip={skip}&limit={limit}");
    }

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        return await _http.GetFromJsonAsync<ProductDto>($"products/{id}");
    }
}

// DTO koji odgovara DummyJSON strukturi
public class ProductsResponse
{
    public List<ProductDto> Products { get; set; } = new();
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal Rating { get; set; }
    public int Stock { get; set; }
    public string Brand { get; set; } = "";
    public string Category { get; set; } = "";
    public string Thumbnail { get; set; } = "";
}
