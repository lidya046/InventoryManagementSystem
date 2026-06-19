using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext db)
    {
        if (db.Categories.Any()) return;

        var electronics = new Category { Name = "Electronics", Description = "Electronic equipment and accessories" };
        var stationery = new Category { Name = "Stationery", Description = "Office and school supplies" };
        var food = new Category { Name = "Food & Beverage", Description = "Packaged food and drinks" };
        db.Categories.AddRange(electronics, stationery, food);
        db.SaveChanges();

        db.Products.AddRange(
            new Product
            {
                Code = "ELC-001", Name = "Wireless Mouse", CategoryId = electronics.Id,
                PurchasePrice = 85000, SellingPrice = 110000, Stock = 20, MinimumStock = 5, Unit = "pcs"
            },
            new Product
            {
                Code = "STN-001", Name = "A4 Notebook", CategoryId = stationery.Id,
                PurchasePrice = 18000, SellingPrice = 25000, Stock = 8, MinimumStock = 10, Unit = "pcs"
            },
            new Product
            {
                Code = "FNB-001", Name = "Mineral Water", CategoryId = food.Id,
                PurchasePrice = 3000, SellingPrice = 5000, Stock = 50, MinimumStock = 12, Unit = "bottle"
            });
        db.SaveChanges();
    }
}
