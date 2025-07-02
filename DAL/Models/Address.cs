using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Address
{
    [Key]
    public int Id { get; set; }
    public string Street { get; set; }
    public string? Landmark { get; set; }

    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(50)]
    public string State { get; set; }

    [MaxLength(10)]
    public string PostalCode { get; set; }
    public bool IsDefault { get; set; } = false;
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual UserDetails UserNavigation { get; set; }
}