

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DAL.ViewModels;
public class RegisterViewModel
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

    // [MaxLength(50, ErrorMessage = "User Name can't be longer than 50 characters")]
    // public string? UserName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number")]
    public string Phone { get; set; } = null!;
    public int UserId { get; set; }
    public string? IdentityUserId { get; set; }
    // public string? ProfileImage { get; set; }

}
