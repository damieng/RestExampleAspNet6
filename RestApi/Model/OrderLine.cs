namespace RestExample.Model;

/// <summary>
/// Details about a line of an order.
/// </summary>
public class OrderLine
{
    /// <summary>
    /// Product being ordered on this line.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Description of the product at time of ordering.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Price of this product at time of ordering.
    /// </summary>
    public Decimal Price { get; set; }
    
    /// <summary>
    /// Quantity of this product being ordered.
    /// </summary>
    public int Quantity { get; set; }
}
