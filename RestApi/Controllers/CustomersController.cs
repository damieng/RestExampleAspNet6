using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestExample.Infrastructure;
using RestExample.Model;

namespace RestExample.Controllers;

/// <summary>
/// REST API controller for managing <see cref="Customer"/> entities.
/// </summary>
[ApiController]
[Authorize]
[Route("/customers")]
public class CustomerController : RestEntityController<Customer>
{
    /// <summary>
    /// Create a new instance of <see cref="CustomerController"/>.
    /// </summary>
    /// <param name="db"><see cref="SampleDbContext"/> for accessing the database.</param>
    public CustomerController(SampleDbContext db)
        : base(db.Customers, () => db.SaveChangesAsync())
    {
    }
}