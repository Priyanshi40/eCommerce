using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace DAL.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum VendorDocuments
{
    PAN,
    Aadhar,
    GST,
    BusinessLicense
}
public class VendorDetails
{
    [Key]
    public int Id { get; set; }
    public int VendorId { get; set; }

    [MaxLength(100)]
    public string BusinessName { get; set; }

    [MaxLength(15)]
    public string GSTNumber { get; set; }
    public string? BusinessAddress { get; set; }
    public int DocumentType { get; set; }
    public string FileUrl { get; set; }

    [ForeignKey(nameof(VendorId))]
    public virtual UserDetails UserNavigation { get; set; }
}