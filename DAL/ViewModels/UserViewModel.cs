
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class UserViewModel
{
    public ICollection<UserDetails> Users { get; set; } = new List<UserDetails>();
    public int Id { get; set; }
    public string? IdentityUserId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    [Required(ErrorMessage = "First Name is Required")]
    [MaxLength(50, ErrorMessage = "First Name can't be longer than 50 characters")]
    [RegularExpression(@"^\S.*$", ErrorMessage = "First Name can't start with a space")]
    public string Firstname { get; set; }
    public string? Lastname { get; set; }
    public bool IsApproved { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}
