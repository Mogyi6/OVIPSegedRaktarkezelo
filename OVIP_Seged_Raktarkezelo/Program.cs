using Logic.Logics.Entities_Logic;
using Logic.Logics.Entities_Logic.Entities_Logic_Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.SOAPClient;
using Repository.Context;

using Repository.Repositories.Entities_Repository;
using Repository.Repositories.Entities_Repository.Entities_Repository_Interfaces;




var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<OvipDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23)),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,  // Maximum retry count
                maxRetryDelay: TimeSpan.FromSeconds(30),  // Maximum delay between retries
                errorNumbersToAdd: null  // SQL error codes to retry on (optional)
            );
        }
    );
});

builder.Services.AddScoped<ICategory_Repository, Category_Repository>();

builder.Services.AddScoped<ICategory_Logic, Category_Logic>();



builder.Services.Configure<OvipOptions>(
    builder.Configuration.GetSection("Ovip"));

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// removed IOvipSoapClient registration - using PHP backend instead

var app = builder.Build();

// apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OvipDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

// Configure the HTTP request pipeline.

    app.UseSwagger();       // <-- Swagger JSON
    app.UseSwaggerUI();     // <-- Swagger UI (böngészőben /swagger)

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var response = new
        {
            title = "Server error",
            detail = exception?.Message,
            path = context.Request.Path,
            trace = app.Environment.IsDevelopment() ? exception?.StackTrace : null
        };

        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
