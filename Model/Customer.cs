using RestExample.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace RestExample.Model;

/// <summary>
/// Details about a customer of the system.
/// </summary>
public class Customer : Entity
{
    /// <summary>
    /// This customers name.
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// A <see cref="List{Address}"/> used by this customer.
    /// </summary>
    public List<Address> Addresses { get; set; } = null!;
}
