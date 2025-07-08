using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? StateId { get; set; }

    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    public virtual ICollection<Address> Address { get; }
}
