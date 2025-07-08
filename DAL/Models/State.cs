using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class State
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? CountryId { get; set; }

    public virtual ICollection<City> Cities { get; }

    [ForeignKey(nameof(CountryId))]
    public virtual Country? Country { get; set; }
    public virtual ICollection<Address> Addresses { get; }
}
