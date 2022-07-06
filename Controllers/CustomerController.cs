using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestExample.Model;

namespace RestExample.Controllers;

/// <summary>
/// REST API controller for managing <see cref="Customer"/> entities.
/// </summary>
[ApiController]
[Route("/customers")]
public class CustomerController : ControllerBase
{
    private readonly SampleDbContext db;

    /// <summary>
    /// Create a new instance of <see cref="CustomerController"/>.
    /// </summary>
    /// <param name="db"><see cref="SampleDbContext"/> for accessing the database.</param>
    public CustomerController(SampleDbContext db)
    {
        this.db = db;
    }

    /// <summary>
    /// Obtain a list of <see cref="Customer"/> entites from storage.
    /// </summary>
    /// <param name="offset">Optional offset to start at based on the <see cref="Customer.CreatedAt"/> .</param>
    /// <param name="limit">Optional maximum number of entities to return.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpGet(Name = "GetCustomers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Index(int? limit, DateTime? offset)
    {
        IQueryable<Customer> query = db.Customers.OrderBy(c => c.CreatedAt);
        if (offset != null)
            query = query.Where(c => c.CreatedAt > offset);
        if (limit != null)
            query = query.Take(limit.Value);

        return Ok(await query.ToListAsync());
    }

    /// <summary>
    /// Retrieve a single <see cref="Customer"/> from storage by it's unique identifier.
    /// </summary>
    /// <param name="id">Unique identifier of desired customer.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
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

    /// <summary>
    /// Delete a <see cref="Customer"/> from storage by it's unique id.
    /// </summary>
    /// <param name="id">Unique identifier of the <see cref="Customer"/> to delete.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
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

    /// <summary>
    /// Create a new <see cref="Customer"/> in storage.
    /// </summary>
    /// <param name="customer"><see cref="Customer"/> entity to store.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpPost(Name = "CreateCustomer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        customer.Id = Guid.NewGuid();
        customer.CreatedAt = DateTime.Now;
        customer.UpdatedAt = DateTime.Now;

        db.Customers.Add(customer);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Update a <see cref="Customer"/> in storage with a new version.
    /// </summary>
    /// <param name="id">Unique identifier of the <see cref="Customer"/> to update.</param>
    /// <param name="customer"><see cref="Customer"/> entity to replace it with.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpPut("{id}", Name = "UpdateCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(Guid id, Customer customer)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var original = db.Customers.FirstOrDefault(c => c.Id == id);
        if (original == null)
            return NotFound();

        customer.Id = id;
        customer.UpdatedAt = DateTime.Now;
        customer.CreatedAt = original.CreatedAt;

        db.Entry(customer).State = EntityState.Modified;
        await db.SaveChangesAsync();

        return NoContent();
    }
}