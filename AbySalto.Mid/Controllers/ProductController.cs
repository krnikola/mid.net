using AbySalto.Mid.Infrastructure.External.DummyJson;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly DummyJsonProductClient _client;

    public ProductController(DummyJsonProductClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int skip = 0, [FromQuery] int limit = 30)
    {
        var result = await _client.GetProductsAsync(skip, limit);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _client.GetProductAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
}