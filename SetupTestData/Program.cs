
using Bogus;
using Microsoft.EntityFrameworkCore;
using RestExample;
using RestExample.Model;

var fakeAddress = new Faker<Address>()
    .RuleFor(a => a.Building, f => f.Address.BuildingNumber())
    .RuleFor(a => a.City, f => f.Address.City())
    .RuleFor(a => a.Country, f => f.Address.City())
    .RuleFor(a => a.PostCode, f => f.Address.ZipCode())
    .RuleFor(a => a.Street, f => f.Address.StreetName())
    .RuleFor(a => a.Region, f => f.Address.Country());

var fakeCustomer = new Faker<Customer>()
    .RuleFor(o => o.Id, f => Guid.NewGuid())
    .RuleFor(o => o.Name, f => f.Name.FindName())
    .RuleFor(o => o.CreatedAt, f => f.Date.Past())
    .RuleFor(o => o.UpdatedAt, f => f.Date.Past(1))
    .RuleFor(o => o.Addresses, f => fakeAddress.GenerateBetween(1, 2));

var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\src\\RestExample\\Example\\Database\\Sample.mdf;Integrated Security=True");

var db = new SampleDbContext(optionsBuilder.Options);

db.Customers.AddRange(fakeCustomer.Generate(100));
db.SaveChanges();