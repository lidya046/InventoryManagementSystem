using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalStock { get; set; }
    public decimal InventoryValue { get; set; }
    public decimal PotentialProfit { get; set; }
    public int HealthyStockCount { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }
    public List<Product> LowStockProducts { get; set; } = [];
    public List<StockTransaction> RecentTransactions { get; set; } = [];
}
