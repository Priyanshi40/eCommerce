using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public bool IsApproved { get; set; } = false;

    [ForeignKey(nameof(IdentityUserId))]
    public virtual IdentityUser IUser { get; set; }
    public virtual ICollection<Address> Addresses { get; } = new List<Address>();


}