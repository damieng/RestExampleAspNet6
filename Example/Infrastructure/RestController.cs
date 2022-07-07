using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestExample.Infrastructure;

/// <summary>
/// REST API controller for managing <typeparamref name="T"/> entities.
/// </summary>
[ApiController]
public abstract class RestController<T> : ControllerBase where T : Entity
{
    private readonly DbSet<T> data;
    private readonly Func<Task> save;

    /// <summary>
    /// Create a new instance of <see cref="RestController<T>"/>.
    /// </summary>
    /// <paramref name="data">Data set these entities belong to.</paramref>
    /// <paramref name="save">Action to perform to commit data upon.</paramref>
    protected RestController(DbSet<T> data, Func<Task> save)
    {
        this.data = data;
        this.save = save;
    }

    /// <summary>
    /// Obtain a list of <typeparamref name="T"/> entites from storage.
    /// </summary>
    /// <param name="offset">Optional offset to start at based on the <see cref="T.CreatedAt"/> .</param>
    /// <param name="limit">Optional maximum number of entities to return.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="IActionResult"/> when complete.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public virtual async Task<ActionResult<List<T>>> Index(int? limit, DateTime? offset)
    {
        var unpagedQuery = data.OrderBy(c => c.CreatedAt);

        IQueryable<T> pagedQuery = unpagedQuery;

        if (offset != null)
            pagedQuery = pagedQuery.Where(c => c.CreatedAt > offset);
        if (limit != null)
            pagedQuery = pagedQuery.Take(limit.Value);

        var results = await pagedQuery.ToListAsync();

        var pagedResults = new PagedResults<T>
        {
            Results = results,
            TotalCount = await unpagedQuery.LongCountAsync()
        };

        return Ok(pagedResults);
    }

    /// <summary>
    /// Retrieve a single <typeparamref name="T"/> from storage by it's unique identifier.
    /// </summary>
    /// <param name="id">Unique identifier of desired entity.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="ActionResult<T>"/> when complete.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<T>> Get(Guid id)
    {
        var entity = await data.FindAsync(id);
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    /// <summary>
    /// Delete a <typeparamref name="T"/> from storage by it's unique id.
    /// </summary>
    /// <param name="id">Unique identifier of the <typeparamref name="T"/> to delete.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="ActionResult"/> when complete.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        var entity = await data.FindAsync(id);
        if (entity == null)
            return NotFound();

        data.Remove(entity);
        await save();

        return NoContent();
    }

    /// <summary>
    /// Create a new <typeparamref name="T"/> in storage.
    /// </summary>
    /// <param name="entity"><typeparamref name="T"/> entity to store.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="ActionResult"/> when complete.</returns>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Create(T entity)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;

        data.Add(entity);
        await save();

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
    }

    /// <summary>
    /// Update a <typeparamref name="T"/> in storage with a new version.
    /// </summary>
    /// <param name="id">Unique identifier of the <typeparamref name="T"/> to update.</param>
    /// <param name="newEntity"><typeparamref name="T"/> entity to replace it with.</param>
    /// <returns>A <see cref="Task"/> that provides a <see cref="ActionResult"/> when complete.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> Update(Guid id, T newEntity)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var originalEntity = data.Find(id);
        if (originalEntity == null)
            return NotFound();

        newEntity.Id = id;
        newEntity.UpdatedAt = DateTime.Now;
        newEntity.CreatedAt = originalEntity.CreatedAt;

        //db.Entry(newEntity).State = EntityState.Modified;
        await save();

        return NoContent();
    }
}