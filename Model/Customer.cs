using System.ComponentModel.DataAnnotations;

namespace RestExample.Model;

public class Customer
{
    public Guid Id { get; set; } 

    [Required]
    public string Name { get; set; } = null!;

    public List<Address> Addresses { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
