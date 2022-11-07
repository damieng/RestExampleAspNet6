using RestExample.Infrastructure;

namespace RestExample.Model;

/// <summary>
/// Details about an order placed.
/// </summary>
public class Order : Entity
{
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
