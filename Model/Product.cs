namespace RestExample.Model;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Decimal Price { get; set; }
}
