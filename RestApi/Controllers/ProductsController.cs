using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestExample.Infrastructure;
using RestExample.Model;

namespace RestExample.Controllers;

/// <summary>
/// REST API controller for managing <see cref="Product"/> entities.
/// </summary>
[ApiController]
[Authorize]
[Route("/products")]
public class ProductsController : RestEntityController<Product>
{
    /// <summary>
    /// Create a new instance of <see cref="ProductsController"/>.
    /// </summary>
    /// <param name="db"><see cref="SampleDbContext"/> for accessing the database.</param>
    public ProductsController(SampleDbContext db)
        : base(db.Products, () => db.SaveChangesAsync())
    {
    }
}