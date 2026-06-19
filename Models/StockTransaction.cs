using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models;

public enum TransactionType
{
    [Display(Name = "Stock In")]
    StockIn = 1,

    [Display(Name = "Stock Out")]
    StockOut = 2
}

public class StockTransaction
{
    public int Id { get; set; }

    [Display(Name = "Product")]
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public TransactionType Type { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [StringLength(250)]
    public string? Note { get; set; }

    [Display(Name = "Transaction Date")]
    public DateTime TransactionDate { get; set; } = DateTime.Now;
}
