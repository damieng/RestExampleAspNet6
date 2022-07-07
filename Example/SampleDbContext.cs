using Microsoft.EntityFrameworkCore;
using RestExample.Model;

namespace RestExample;

public class SampleDbContext : DbContext
{
    public DbSet<Customer> Customers { get; init; } = null!;
    public DbSet<Order> Orders { get; init; } = null!;

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().OwnsMany(o => o.Addresses);

        modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress);
        modelBuilder.Entity<Order>().OwnsMany(o => o.OrderLines);
    }
}
