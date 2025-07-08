using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Models;
public class Product
{
    [Key]
    public int Id { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public string? AdminComment { get; set; }
    public string? CoverImage { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; }
    public ICollection<ProductImage>? Images { get; set; }
    public ICollection<Wishlist>? WishList { get; set; }


}