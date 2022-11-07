using RestExample.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace RestExample.Model;
/// <summary>
/// Entity that represents a product.
/// </summary>
public class Product : Entity
{
    /// <summary>
    /// Unique identifier used to reference this product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of this product.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Price of this product.
    /// </summary>
    public Decimal Price { get; set; }
}
