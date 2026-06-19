using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(30)]
    [Display(Name = "Product Code")]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(150)]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Category")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Purchase price cannot be negative.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Purchase Price")]
    public decimal PurchasePrice { get; set; }

    [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Selling price cannot be negative.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Selling Price")]
    public decimal SellingPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Minimum Stock")]
    public int MinimumStock { get; set; }

    [Required, StringLength(20)]
    public string Unit { get; set; } = "pcs";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
