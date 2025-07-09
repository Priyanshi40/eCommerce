using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Address
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Street { get; set; }

    [MaxLength(50)]
    public string? Landmark { get; set; }

    [MaxLength(50)]
    public string HouseName { get; set; }
    public int CityId { get; set; }
    [ForeignKey(nameof(CityId))]
    public virtual City City { get; set; }
    public int StateId { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    public int CountryId { get; set; }
    [ForeignKey(nameof(CountryId))]
    public virtual Country Country { get; set; }

    [MaxLength(10)]
    public string PostalCode { get; set; }
    public bool IsDefault { get; set; } = false;
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserDetails UserNavigation { get; set; }
    public DateTime Createdat { get; set; }
    public DateTime Modifiedat { get; set; }
    public int ModifiedBy { get; set; }
    public int CreatedBy { get; set; }
}