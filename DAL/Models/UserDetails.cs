using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models;

public class UserDetails
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Firstname { get; set; }

    [MaxLength(50)]
    public string? Lastname { get; set; }
    public string IdentityUserId { get; set; }
    public DateTime Createdat { get; set; }
    public DateTime Modifiedat { get; set; }
    public DateTime Modifieby { get; set; }
    public bool IsApproved { get; set; } = false;
    public ProductStatus Status { get; set; }
    public string? AdminComment { get; set; }

    [ForeignKey(nameof(IdentityUserId))]
    public virtual IdentityUser IUser { get; set; }
    public virtual ICollection<Address> Addresses { get; } = new List<Address>();


}