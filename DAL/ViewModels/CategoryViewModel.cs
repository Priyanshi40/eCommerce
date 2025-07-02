using System.ComponentModel.DataAnnotations;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAL.ViewModels;

public enum CategoryStatus
{
    Active = 1,
    Deactive = 2
}
public class CategoryViewModel
{
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    public int Id { get; set; }

    [Required(ErrorMessage = "Category Name is required")]
    [Remote("CheckDuplicateName", "Category", AdditionalFields = "Id", ErrorMessage = "Category name already exists!")]
    public string Name { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(200, ErrorMessage = "Description can't be longer than 200 characters")]
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public bool IsActive { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }

}
