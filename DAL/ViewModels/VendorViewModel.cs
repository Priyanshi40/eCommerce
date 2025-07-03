using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class VendorViewModel
{
    public int UserId { get; set; }
    public string? IdentityUserId { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Business Name can't be longer than 100 characters")]
    public string BusinessName { get; set; }

    [Required (ErrorMessage = "GST Number is required")]
    [RegularExpression
        (@"([0][1-9]|[1-2][0-9]|[3][0-7])([a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}[1-9a-zA-Z]{1}[zZ]{1}[0-9a-zA-Z]{1})+$", ErrorMessage = "Invalid GST Number format")]
    [MaxLength(15, ErrorMessage = "GST Number can't be longer than 15 characters")]
    public string? GSTNumber { get; set; }

    [MaxLength(200, ErrorMessage = "Business Address can't be longer than 200 characters")]
    public string? BusinessAddress { get; set; }

    [Required(ErrorMessage = "Document Type is required")]
    public VendorDocuments DocumentType { get; set; }

    [Required(ErrorMessage = "Document is required")]
    public string FileUrl { get; set; }
    public int Id { get; set; }
    public int VendorId { get; set; }
}
