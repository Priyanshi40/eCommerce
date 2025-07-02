using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Decription { get; set; } = null!;
    public bool Isdeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}