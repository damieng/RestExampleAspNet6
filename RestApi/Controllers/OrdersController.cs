//using Microsoft.AspNetCore.Mvc;
//using RestExample.Infrastructure;
//using RestExample.Model;

//namespace RestExample.Controllers;

///// <summary>
///// REST API controller for managing <see cref="Order"/> entities.
///// </summary>
//[ApiController]
//[Route("/orders")]
//public class OrdersController : RestEntityController<Order>
//{
//    /// <summary>
//    /// Create a new instance of <see cref="OrdersController"/>.
//    /// </summary>
//    /// <param name="db"><see cref="SampleDbContext"/> for accessing the database.</param>
//    public OrdersController(SampleDbContext db)
//        : base(db.Orders, () => db.SaveChangesAsync())
//    {
//    }
//}