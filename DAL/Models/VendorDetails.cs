using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public enum VendorDocuments
{
    PAN = 1,
    Aadhar = 2,
    GST = 3,
    BusinessLicense = 4,
    Other = 5
}
public class VendorDetails
{
    [Key]
    public int Id { get; set; }
    public int VendorId { get; set; }

    [MaxLength(100)]
    public string BusinessName { get; set; }
    public string? GSTNumber { get; set; }
    public string? BusinessAddress { get; set; }
    public VendorDocuments DocumentType { get; set; }

    [MaxLength(50)]
    public string DocumentName { get; set; }
    public string FileUrl { get; set; }

    [ForeignKey(nameof(VendorId))]
    public virtual UserDetails UserNavigation { get; set; }
}