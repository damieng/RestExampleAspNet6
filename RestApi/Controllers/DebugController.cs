using Microsoft.AspNetCore.Mvc;
using RestApi.Configuration;
using RestApi.Infrastructure;

namespace RestExample.Controllers;

/// <summary>
/// API controller for debugging and developing purposes.
/// </summary>
/// <remarks>Do not ship this controller with your solution to prod or staging environments.</remarks>
[ApiController]
[Route("/debug")]
public class DebugController : Controller
{
    readonly IConfiguration configuration;

    /// <summary>
    /// Create a new instance of <see cref="DebugController"/>.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/> that provides application configuration information.</param>
    public DebugController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Creates a JWT token for use in developing and testing the API.
    /// </summary>
    /// <returns>JWT token for testing.</returns>
    [HttpGet("/createJwt")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public string CreateJwt(Guid? userId, string? name, [FromQuery(Name = "role")] string[] roles)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();

        var claims = AuthenticationHelper.CreateClaims(userId ?? Guid.NewGuid(), name ?? "Default Joe", roles ?? new[] { "user" });
        var token = AuthenticationHelper.CreateJwtToken(jwtOptions, claims);

        return AuthenticationHelper.SerializeJwtToken(token);
    }
}