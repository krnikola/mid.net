using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AbySalto.Mid.Infrastructure.External.DummyJson;

namespace AbySalto.Mid.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly DummyJsonProductClient _productClient;

    public FavoritesController(AppDbContext context, DummyJsonProductClient productClient)
    {
        _context = context;
        _productClient = productClient;
    }

    [HttpPost("{productId:int}")]
    public async Task<IActionResult> Add(int productId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var exists = _context.Favorites.Any(f => f.UserId == userId && f.ProductId == productId);
        if (exists) return Ok("Already in favorites.");

        _context.Favorites.Add(new Favorite
        {
            UserId = userId,
            ProductId = productId
        });

        await _context.SaveChangesAsync();
        return Ok("Added to favorites.");
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Remove(int productId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var fav = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
        if (fav == null) return NotFound();

        _context.Favorites.Remove(fav);
        await _context.SaveChangesAsync();

        return Ok("Removed from favorites.");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var productIds = _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.ProductId)
            .ToList();

        var tasks = productIds.Select(async id =>
        {
            try 
            { 
                return await _productClient.GetProductAsync(id); 
            }
            catch 
            { 
                return null; 
            }
        });

        var results = await Task.WhenAll(tasks);
        return Ok(results.Where(p => p != null));
    }
}