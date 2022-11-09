
using Bogus;
using Microsoft.EntityFrameworkCore;
using RestExample;
using RestExample.Model;

var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\src\\RestExample\\Database\\Sample.mdf;Integrated Security=True");
var db = new SampleDbContext(optionsBuilder.Options);

var fakeAddress = new Faker<Address>()
    .RuleFor(a => a.Building, f => f.Address.BuildingNumber())
    .RuleFor(a => a.City, f => f.Address.City())
    .RuleFor(a => a.Country, f => f.Address.City())
    .RuleFor(a => a.PostCode, f => f.Address.ZipCode())
    .RuleFor(a => a.Street, f => f.Address.StreetName())
    .RuleFor(a => a.Region, f => f.Address.Country());

var fakeCustomer = new Faker<Customer>()
    .RuleFor(c => c.Id, f => Guid.NewGuid())
    .RuleFor(c => c.Name, f => f.Name.FindName())
    .RuleFor(c => c.CreatedAt, f => f.Date.Past(1))
    .RuleFor(c => c.Addresses, f => fakeAddress.GenerateBetween(1, 2))
    .Rules((f, c) => {
         c.UpdatedAt = f.Date.Between(c.CreatedAt, DateTime.UtcNow);
     });

var fakeProducts = new Faker<Product>()
    .RuleFor(p => p.Id, f => Guid.NewGuid())
    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
    .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
    .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
    .Rules((f, p) => {
        p.UpdatedAt = f.Date.Between(p.CreatedAt, DateTime.UtcNow);
    });

var allCustomers = fakeCustomer.Generate(100);
var allProducts = fakeProducts.Generate(100);

db.Customers.AddRange(allCustomers);
db.Products.AddRange(allProducts);
db.SaveChanges();

// TODO: Make orders work 

var fakeOrderLines = new Faker<OrderLine>()
    .RuleFor(l => l.Quantity, f => f.Random.Int(1, 25))
    .Rules((f, l) =>
    {
        l.Product = f.PickRandom(allProducts);
        l.Price = l.Product.Price;
        l.Description = l.Product.Name;
    });

var fakeOrders = new Faker<Order>()
    .RuleFor(o => o.Id, f => Guid.NewGuid())
    .RuleFor(o => o.Customer, f => f.PickRandom(allCustomers))
    .RuleFor(o => o.CreatedAt, f => f.Date.Past(2))
    .RuleFor(o => o.ShippingAddress, f => f.PickRandom(f.PickRandom(allCustomers).Addresses))
    .RuleFor(o => o.OrderLines, f => fakeOrderLines.GenerateBetween(1, 8))
    .Rules((f, o) => {
      o.UpdatedAt = f.Date.Between(o.CreatedAt, DateTime.UtcNow);
    });

var allOrders = fakeOrders.Generate(25);

