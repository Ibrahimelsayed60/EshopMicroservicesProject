using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Catalog.API.Data;
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
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            });

            builder.Services.AddValidatorsFromAssembly(assembly);
            builder.Services.AddCarter();

            builder.Services.AddMarten(opts =>
            {
                opts.Connection(builder.Configuration.GetConnectionString("Database")!);
                
            }).UseLightweightSessions();

            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            if (builder.Environment.IsDevelopment())
                builder.Services.InitializeMartenWith<CatalogInitialData>();

            //builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            //builder.Services.AddHealthChecks()
            //    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

            var app = builder.Build();

            // Configure the HTTP request Pipeline.
            app.MapCarter();

            app.UseExceptionHandler(options => { });

            //app.UseHealthChecks("/health",
            //    new HealthCheckOptions
            //    {
            //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //    });

            app.Run();
        }
    }
}
