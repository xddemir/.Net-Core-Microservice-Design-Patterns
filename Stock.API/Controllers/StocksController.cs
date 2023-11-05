using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Controllers;

[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly AppDbContext _context;

    public StocksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await _context.Stocks.ToListAsync());
    }
}