using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers;

public class CategoriesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.Categories
            .Include(c => c.Products)
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (await context.Categories.AnyAsync(c => c.Name == category.Name))
            ModelState.AddModelError(nameof(category.Name), "Category name already exists.");

        if (!ModelState.IsValid) return View(category);

        context.Add(category);
        await context.SaveChangesAsync();
        TempData["Success"] = "Category added successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await context.Categories.FindAsync(id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id) return NotFound();

        if (await context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id))
            ModelState.AddModelError(nameof(category.Name), "Category name already exists.");

        if (!ModelState.IsValid) return View(category);

        context.Update(category);
        await context.SaveChangesAsync();
        TempData["Success"] = "Category updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (category is null) return NotFound();

        if (category.Products.Count > 0)
        {
            TempData["Error"] = "Category cannot be deleted because it is used by products.";
            return RedirectToAction(nameof(Index));
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        TempData["Success"] = "Category deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
