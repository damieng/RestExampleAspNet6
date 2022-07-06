namespace RestExample.Model;

/// <summary>
/// Details about an order placed.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique reference for this order.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// When this order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When this order was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Which <see cref="Customer"/> placed this order.
    /// </summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>
    /// The desired shipping <see cref="Address"/> for this order.
    /// </summary>
    public Address ShippingAddress { get; set; } = null!;

    /// <summary>
    /// Lines of the order indicating the items and quantities of ordered goods.
    /// </summary>
    public List<OrderLine> OrderLines { get; set; } = null!;
}
