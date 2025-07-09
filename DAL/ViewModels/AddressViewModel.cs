using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class AddressViewModel
{
    public ICollection<AddressViewModel> Address { get; set; } = new List<AddressViewModel>();
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Street can't be longer than 100 characters")]
    public string Street { get; set; }

    [MaxLength(50, ErrorMessage = "Landmark can't be longer than 50 characters")]
    public string? Landmark { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "House Name can't be longer than 50 characters")]
    public string HouseName { get; set; }
    public int CityId { get; set; }
    public int StateId { get; set; }
    public int CountryId { get; set; }

    [RegularExpression(@"([1-9]{1}[0-9]{5}|[1-9]{1}[0-9]{3}\\s[0-9]{3})", ErrorMessage = "Enter Valid Postal Code")]
    public string PostalCode { get; set; }
    public string? CityName { get; set; }
    public string? StateName { get; set; }
    public string? CountryName { get; set; }
    public bool IsDefault { get; set; } = false;
    public int UserId { get; set; }
    public int ModifiedBy { get; set; }
    public int CreatedBy { get; set; }
}