using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class VendorViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "First Name is Required")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number")]
    public string Phone { get; set; } = null!;
    public int UserId { get; set; }
    public string? IdentityUserId { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Business Name can't be longer than 100 characters")]
    public string BusinessName { get; set; }
    public string? GSTNumber { get; set; }

    [MaxLength(200, ErrorMessage = "Business Address can't be longer than 200 characters")]
    public string? BusinessAddress { get; set; }

    [Required (ErrorMessage = "Document Type is required")]
    public VendorDocuments DocumentType { get; set; }

    [Required (ErrorMessage = "Document Name is required")]
    [MaxLength(50, ErrorMessage = "Document Name can't be longer than 50 characters")]
    public string DocumentName { get; set; }

    [Required (ErrorMessage = "Document is required")]
    public string FileUrl { get; set; }
}
