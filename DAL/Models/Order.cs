using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public enum OrderStatus
{
    Placed,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
public class Order
{
    [Key]
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }

    [MaxLength(500)]
    public string Comment { get; set; }
    public int UserId { get; set; }
    public int AddressId { get; set; } 
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserDetails UserNavigation { get; set; }

    [ForeignKey(nameof(AddressId))]
    public virtual Address Address { get; set; }
    public virtual ICollection<OrderProduct> OrderProducts { get; set; }
}