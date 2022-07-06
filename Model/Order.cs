namespace RestExample.Model;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderedAt { get; set; }
    public Customer Customer { get; set; } = null!;
    public Address ShippingAddress { get; set; } = null!;
    public List<OrderLine> OrderLines { get; set; } = null!;
}
