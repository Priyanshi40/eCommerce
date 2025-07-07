using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(CartId))]
    public virtual Cart CartNavigation { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product ProductNavigation { get; set; }
}
