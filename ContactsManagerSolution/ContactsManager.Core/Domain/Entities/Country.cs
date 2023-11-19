using System.ComponentModel.DataAnnotations;

namespace Entities;
public class Country
{
    /// <summary>
    /// Domain Model for storing Country Details
    /// </summary>

    [Key]
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public virtual ICollection<Person>? Persons { get; set; }

}

