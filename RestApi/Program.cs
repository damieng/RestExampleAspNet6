using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SystemTextJsonPatch.Converters;

namespace RestExample;

/// <summary>
/// Initial program entry class;
/// </summary>
public class Program
{
    /// <summary>
    /// Initial program entry method.
    /// </summary>
    /// <param name="args">Arguments provided to the application.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonPatchDocumentConverterFactory());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // It uses BadRequest not UnprocessableEntity
            });

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Example REST WebAPI",
                Version = "v1"
            });
        });

        services.AddDbContext<SampleDbContext>(options =>
            options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\src\\RestExample\\Database\\Sample.mdf;Integrated Security=True"));

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

