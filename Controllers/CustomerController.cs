using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestExample.Model;

namespace RestExample.Controllers;

[ApiController]
[Route("/customers")]
public class CustomerController : ControllerBase
{
    private readonly SampleDbContext db;

    public CustomerController(SampleDbContext db)
    {
        this.db = db;
    }

    [HttpGet(Name = "GetCustomers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Index()
    {
        var customers = await db.Customers.OrderBy(c => c.CreatedAt).ToListAsync();
        return Ok(customers);
    }

    [HttpGet("{id}", Name = "GetCustomer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var customer = await db.Customers.FindAsync(id);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpDelete("{id}", Name = "DeleteCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var customer = await db.Customers.FindAsync(id);
        if (customer == null)
            return NotFound();

        db.Customers.Remove(customer);
        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost(Name = "CreateCustomer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        customer.Id = Guid.NewGuid();
        customer.CreatedAt = DateTime.Now;

        db.Customers.Add(customer);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}", Name = "ReplaceCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Replace(Guid id, Customer customer)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        customer.CreatedAt = DateTime.Now;
        customer.Id = id;

        try
        {
            db.Entry(customer).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException) {
            var exists = await db.Customers.AnyAsync(c => c.Id == id);
            if (!exists)
                return NotFound();
        }

        return NoContent();
    }
}