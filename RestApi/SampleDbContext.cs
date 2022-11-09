using Microsoft.EntityFrameworkCore;
using RestExample.Model;

namespace RestExample;

/// <summary>
/// Database context for accessing the sample data.
/// </summary>
public class SampleDbContext : DbContext
{
    /// <summary>
    /// Customers stored in the database.
    /// </summary>
    public DbSet<Customer> Customers { get; init; } = null!;

    /// <summary>
    /// Orders stored in the database.
    /// </summary>
    public DbSet<Order> Orders { get; init; } = null!;

    /// <summary>
    /// Products stored in the database.
    /// </summary>
    public DbSet<Product> Products { get; init; } = null!;

    /// <summary>
    /// Initialize a new instance of <see cref="SampleDbContext"/> with the given options.
    /// </summary>
    /// <param name="options">A <see cref="DbContextOptions{SampleDbContext}"/> for configuring this context.</param>
    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().OwnsMany(o => o.Addresses).ToTable("CustomerAddresses");

        modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress);
        modelBuilder.Entity<Order>().OwnsMany(o => o.OrderLines).ToTable("OrderLines");
    }
}
