using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserDetails UserNavigation { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
