using InventoryManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.ViewModels;

public class StockTransactionViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a product."), Display(Name = "Product")]
    public int ProductId { get; set; }

    [Required, Display(Name = "Transaction Type")]
    public TransactionType Type { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [StringLength(250)]
    public string? Note { get; set; }
}
