namespace RestExample.Model;

/// <summary>
/// A physical address used by <see cref="Customer"/> and <see cref="Order"/>.
/// </summary>
public class Address
{
    /// <summary>
    /// Apartment or suite number for this address.
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Building number or name.
    /// </summary>
    public string Building { get; set;} = null!;

    /// <summary>
    /// Street this building resides on.
    /// </summary>
    public string Street { get; set; } = null!;

    /// <summary>
    /// City this street exists within.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Region this address resides within.
    /// </summary>
    public string Region { get; set; } = null!;

    /// <summary>
    /// Postal or ZIP code for this address.
    /// </summary>
    public string PostCode { get; set; } = null!;

    /// <summary>
    /// Country this address resides within.
    /// </summary>
    public string Country { get; set; } = null!;
}
