using System.ComponentModel.DataAnnotations;

namespace RestExample.Model;

/// <summary>
/// Details about a customer of the system.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier used to reference this customer.
    /// </summary>
    public Guid Id { get; set; } 

    /// <summary>
    /// This customers name.
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// A <see cref="List{Address}"/> used by this customer.
    /// </summary>
    public List<Address> Addresses { get; set; } = null!;

    /// <summary>
    /// When this customer was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When this customer was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
