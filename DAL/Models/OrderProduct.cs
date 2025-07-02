using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class OrderProduct
{
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; }
}