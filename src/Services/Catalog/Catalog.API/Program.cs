using BuildingBlocks.Behaviors;
using Carter;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services to DI Container.
            

            var assembly = typeof(Program).Assembly;

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));

            });

            builder.Services.AddValidatorsFromAssembly(assembly);
            builder.Services.AddCarter();

            builder.Services.AddMarten(opts =>
            {
                opts.Connection(builder.Configuration.GetConnectionString("Database")!);
                
            }).UseLightweightSessions();


            //if (builder.Environment.IsDevelopment())
            //    builder.Services.InitializeMartenWith<CatalogInitialData>();

            //builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            //builder.Services.AddHealthChecks()
            //    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

            var app = builder.Build();

            // Configure the HTTP request Pipeline.
            app.MapCarter();

            //app.UseHealthChecks("/health",
            //    new HealthCheckOptions
            //    {
            //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //    });

            app.Run();
        }
    }
}
