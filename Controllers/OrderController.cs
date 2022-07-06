using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestExample.Model;

namespace RestExample.Controllers;

/// <summary>
/// REST API controller for managing <see cref="Order"/> entities.
/// </summary>
[ApiController]
[Route("/orders")]
public class OrderController : ControllerBase
{
    private readonly SampleDbContext db;

    /// <summary>
    /// Create a new instance of <see cref="OrderController"/>.
    /// </summary>
    /// <param name="db"><see cref="SampleDbContext"/> for accessing the database.</param>
    public OrderController(SampleDbContext db)
    {
        this.db = db;
    }

    /// <summary>
    /// Obtain a list of <see cref="Order"/> entites from storage.
    /// </summary>
    /// <param name="offset">Optional offset to start at based on the <see cref="Order.CreatedAt"/> .</param>
    /// <param name="limit">Optional maximum number of entities to return.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpGet(Name = "GetOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Index(int? limit, DateTime? offset)
    {
        IQueryable<Order> query = db.Orders.OrderBy(c => c.CreatedAt);
        if (offset != null)
            query = query.Where(c => c.CreatedAt > offset);
        if (limit != null)
            query = query.Take(limit.Value);

        return Ok(await query.ToListAsync());
    }

    /// <summary>
    /// Retrieve a single <see cref="Order"/> from storage by it's unique identifier.
    /// </summary>
    /// <param name="id">Unique identifier of desired order.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpGet("{id}", Name = "GetOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var order = await db.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    /// <summary>
    /// Delete a <see cref="Order"/> from storage by it's unique id.
    /// </summary>
    /// <param name="id">Unique identifier of the <see cref="Order"/> to delete.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpDelete("{id}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var order = await db.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        db.Orders.Remove(order);
        await db.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Create a new <see cref="Order"/> in storage.
    /// </summary>
    /// <param name="order"><see cref="Order"/> entity to store.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpPost(Name = "CreateOrder")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(Order order)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.Now;
        order.UpdatedAt = DateTime.Now;

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    /// <summary>
    /// Update a <see cref="Order"/> in storage with a new version.
    /// </summary>
    /// <param name="id">Unique identifier of the <see cref="Order"/> to update.</param>
    /// <param name="order"><see cref="Order"/> entity to replace it with.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [HttpPut("{id}", Name = "UpdateOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(Guid id, Order order)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var original = db.Orders.FirstOrDefault(c => c.Id == id);
        if (original == null)
            return NotFound();

        order.Id = id;
        order.UpdatedAt = DateTime.Now;
        order.CreatedAt = original.CreatedAt;

        db.Entry(order).State = EntityState.Modified;
        await db.SaveChangesAsync();

        return NoContent();
    }
}