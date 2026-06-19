using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers;

public class ProductsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? search, int? categoryId)
    {
        var query = context.Products.Include(p => p.Category).AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || p.Code.Contains(search));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        ViewBag.Search = search;
        ViewBag.CategoryId = new SelectList(await context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", categoryId);
        return View(await query.OrderBy(p => p.Name).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product is null ? NotFound() : View(product);
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategories();
        return View(new Product());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (await context.Products.AnyAsync(p => p.Code == product.Code))
            ModelState.AddModelError(nameof(product.Code), "Product code already exists.");

        if (!ModelState.IsValid)
        {
            await LoadCategories(product.CategoryId);
            return View(product);
        }

        product.CreatedAt = product.UpdatedAt = DateTime.Now;
        context.Products.Add(product);
        await context.SaveChangesAsync();
        TempData["Success"] = "Product added successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null) return NotFound();
        await LoadCategories(product.CategoryId);
        return View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product form)
    {
        if (id != form.Id) return NotFound();

        if (await context.Products.AnyAsync(p => p.Code == form.Code && p.Id != id))
            ModelState.AddModelError(nameof(form.Code), "Product code already exists.");

        if (!ModelState.IsValid)
        {
            await LoadCategories(form.CategoryId);
            return View(form);
        }

        var product = await context.Products.FindAsync(id);
        if (product is null) return NotFound();

        product.Code = form.Code;
        product.Name = form.Name;
        product.CategoryId = form.CategoryId;
        product.PurchasePrice = form.PurchasePrice;
        product.SellingPrice = form.SellingPrice;
        product.MinimumStock = form.MinimumStock;
        product.Unit = form.Unit;
        product.UpdatedAt = DateTime.Now;

        await context.SaveChangesAsync();
        TempData["Success"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await context.Products.Include(p => p.StockTransactions).FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) return NotFound();

        if (product.StockTransactions.Count > 0)
        {
            TempData["Error"] = "Product cannot be deleted because it already has stock transactions.";
            return RedirectToAction(nameof(Index));
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        TempData["Success"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategories(int? selectedId = null)
    {
        ViewBag.CategoryId = new SelectList(await context.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", selectedId);
    }
}
