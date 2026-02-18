using AbySalto.Mid.Infrastructure.Services.Stores;
using AbySalto.Mid.WebApi.Features.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbySalto.Mid.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BasketController : ControllerBase
{
    private readonly BasketStore _basketStore;

    public BasketController(BasketStore basketStore)
    {
        _basketStore = basketStore;
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddToBasketRequest request)
    {
        if (request.Quantity <= 0) return BadRequest("Quantity must be > 0.");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _basketStore.AddItemAsync(userId, request.ProductId, request.Quantity);
        return Ok("Item added.");
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(int productId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var removed = await _basketStore.RemoveItemAsync(userId, productId);
        return removed ? Ok("Item removed.") : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetBasket()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var items = await _basketStore.GetItemsAsync(userId);

        var response = items.Select(i => new { productId = i.ProductId, quantity = i.Quantity });
        return Ok(response);
    }
}
