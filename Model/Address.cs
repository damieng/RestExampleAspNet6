namespace RestExample.Model;

public class Address
{
    public string? Unit { get; set; }
    public string Building { get; set;} = null!;
    public string Street { get; set; } = null!;
    public string? City { get; set; }
    public string Region { get; set; } = null!;
    public string PostCode { get; set; } = null!;
    public string Country { get; set; } = null!;
}
