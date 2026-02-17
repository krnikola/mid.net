using AbySalto.Mid.Infrastructure.Persistence;
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
    private readonly AppDbContext _context;
    
    public BasketController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(AddToBasketRequest request)
    {
        if (request.Quantity <= 0) return BadRequest("Quantity must be > 0.");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var existing = _context.CartItems.FirstOrDefault(x => x.UserId == userId && x.ProductId == request.ProductId);

        if (existing == null)
        {
            _context.CartItems.Add(new CartItem
            {
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UpdatedAt = DateTime.UtcNow
            });
        }
        else
        {
            existing.Quantity += request.Quantity;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok("Item added.");
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(int productId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var item = _context.CartItems.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
        if (item == null) return NotFound();

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok("Item removed.");
    }

    [HttpGet]
    public IActionResult GetBasket()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var items = _context.CartItems
            .Where(x => x.UserId == userId)
            .Select(x => new { x.ProductId, x.Quantity })
            .ToList();

        return Ok(items);
    }
}