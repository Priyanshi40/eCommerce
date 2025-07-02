using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Wishlist
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserDetails User { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; }
}