using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers;

public class StockTransactionsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var transactions = await context.StockTransactions
            .Include(t => t.Product)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
        return View(transactions);
    }

    public async Task<IActionResult> Create(int? productId)
    {
        await LoadProducts(productId);
        return View(new StockTransactionViewModel { ProductId = productId ?? 0, Type = TransactionType.StockIn, Quantity = 1 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StockTransactionViewModel model)
    {
        var product = await context.Products.FindAsync(model.ProductId);
        if (product is null)
            ModelState.AddModelError(nameof(model.ProductId), "Product not found.");

        if (product is not null && model.Type == TransactionType.StockOut && model.Quantity > product.Stock)
            ModelState.AddModelError(nameof(model.Quantity), $"Insufficient stock. Available stock: {product.Stock} {product.Unit}.");

        if (!ModelState.IsValid)
        {
            await LoadProducts(model.ProductId);
            return View(model);
        }

        product!.Stock += model.Type == TransactionType.StockIn ? model.Quantity : -model.Quantity;
        product.UpdatedAt = DateTime.Now;

        context.StockTransactions.Add(new StockTransaction
        {
            ProductId = model.ProductId,
            Type = model.Type,
            Quantity = model.Quantity,
            Note = model.Note,
            TransactionDate = DateTime.Now
        });

        await context.SaveChangesAsync();
        TempData["Success"] = "Stock transaction saved successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadProducts(int? selectedId = null)
    {
        var products = await context.Products.OrderBy(p => p.Name).ToListAsync();
        ViewBag.ProductId = new SelectList(
            products.Select(p => new { p.Id, Display = $"{p.Code} - {p.Name} (Stock: {p.Stock})" }),
            "Id", "Display", selectedId);
    }
}
