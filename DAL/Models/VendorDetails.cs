using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class VendorDetails
{
    [Key]
    public int Id { get; set; }
    public int VendorId { get; set; }

    [MaxLength(50)]
    public string BusinessName { get; set; }
    public string? GSTNumber { get; set; }
    public string? BusinessAddress { get; set; }
    public string DocumentName { get; set; }
    public string FileUrl { get; set; }

    [ForeignKey(nameof(VendorId))]
    public virtual UserDetails UserNavigation { get; set; }
}