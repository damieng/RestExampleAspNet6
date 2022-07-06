namespace RestExample.Model;

public class OrderLine
{
    public Product Product { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Decimal Price { get; set; }
    public int Quantity { get; set; }
}
