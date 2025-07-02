using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(250)]
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; } = true;
    public string? CoverImage { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}