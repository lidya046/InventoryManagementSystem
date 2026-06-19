using InventoryManagementSystem.Data;
using InventoryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public IActionResult Error() => View();

    public async Task<IActionResult> Index()
    {
        var products = await context.Products.AsNoTracking().ToListAsync();
        var model = new DashboardViewModel
        {
            TotalProducts = products.Count,
            TotalCategories = await context.Categories.CountAsync(),
            TotalStock = products.Sum(p => p.Stock),
            InventoryValue = products.Sum(p => p.PurchasePrice * p.Stock),
            PotentialProfit = products.Sum(p => (p.SellingPrice - p.PurchasePrice) * p.Stock),
            HealthyStockCount = products.Count(p => p.Stock > p.MinimumStock),
            LowStockCount = products.Count(p => p.Stock > 0 && p.Stock <= p.MinimumStock),
            OutOfStockCount = products.Count(p => p.Stock == 0),
            LowStockProducts = products
                .Where(p => p.Stock <= p.MinimumStock)
                .OrderBy(p => p.Stock)
                .Take(6)
                .ToList(),
            RecentTransactions = await context.StockTransactions
                .Include(t => t.Product)
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionDate)
                .Take(8)
                .ToListAsync()
        };

        return View(model);
    }
}
