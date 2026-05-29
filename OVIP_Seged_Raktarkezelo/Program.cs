
using Logic.Logic.CategoriesLogic;
using Logic.Logic.CategoriesLogic.Interfaces;
using Logic.Logic.ManufactureLogic;
using Logic.Logic.ManufactureLogic.Interfaces;
using Logic.Logic.ParametersLogic;
using Logic.Logic.ParametersLogic.Interfaces;
using Logic.Logic.PricingLogic;
using Logic.Logic.PricingLogic.Interfaces;
using Logic.Logic.ProductsLogic;
using Logic.Logic.ProductsLogic.Interfaces;
using Logic.Logic.SOAPClient;
using Microsoft.EntityFrameworkCore;
using Models.SOAPClient;
using Repository.Context;
using Repository.Mappings;
using Repository.Repository;
using Repository.Repository.CategoriesRepository;
using Repository.Repository.CategoriesRepository.Interfaces;
using Repository.Repository.ManufactureRepositroy;
using Repository.Repository.ManufactureRepositroy.Interfaces;
using Repository.Repository.ParametersRepositroy;
using Repository.Repository.ParametersRepositroy.Interfaces;
using Repository.Repository.PricingRepository;
using Repository.Repository.PricingRepository.Interfaces;
using Repository.Repository.ProductsRepository;
using Repository.Repository.ProductsRepository.Interfaces;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.

builder.Services.AddControllers();

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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.Configure<OvipOptions>(
    builder.Configuration.GetSection("Ovip"));

builder.Services.AddHttpClient<IOvipSoapClient, OvipSoapClient>();

builder.Services.AddScoped<IOvipSyncLogic, OvipSyncLogic>();


// =========================
// PRODUCTS
// =========================
builder.Services.AddTransient<IOvipProductRepository, OvipProductRepository>();
builder.Services.AddTransient<IOvipProductImageRepository, OvipProductImageRepository>();
builder.Services.AddTransient<IOvipProductParameterRepository, OvipProductParameterRepository>();
builder.Services.AddTransient<IOvipStockRepository, OvipStockRepository>();

builder.Services.AddScoped<IOvipProductLogic, OvipProductLogic>();
builder.Services.AddScoped<IOvipProductImageLogic, OvipProductImageLogic>();
builder.Services.AddScoped<IOvipProductParameterLogic, OvipProductParameterLogic>();
builder.Services.AddScoped<IOvipStockLogic, OvipStockLogic>();

// =========================
// CATEGORIES
// =========================
builder.Services.AddTransient<IOvipCategoryRepository, OvipCategoryRepository>();
builder.Services.AddTransient<IOvipCategoryConnectionRepository, OvipCategoryConnectionRepository>();

builder.Services.AddScoped<IOvipCategoryLogic, OvipCategoryLogic>();
builder.Services.AddScoped<IOvipCategoryConnectionLogic, OvipCategoryConnectionLogic>();

// =========================
// PARAMETERS
// =========================
builder.Services.AddTransient<IOvipParameterRepository, OvipParameterRepository>();

builder.Services.AddScoped<IOvipParameterLogic, OvipParameterLogic>();

// =========================
// PRICING
// =========================
builder.Services.AddTransient<IOvipPriceListRepository, OvipPriceListRepository>();
builder.Services.AddTransient<IOvipPriceListPriceRepository, OvipPriceListPriceRepository>();
builder.Services.AddTransient<IOvipQuantityDiscountRepository, OvipQuantityDiscountRepository>();

builder.Services.AddScoped<IOvipPriceListLogic, OvipPriceListLogic>();
builder.Services.AddScoped<IOvipPriceListPriceLogic, OvipPriceListPriceLogic>();
builder.Services.AddScoped<IOvipQuantityDiscountLogic, OvipQuantityDiscountLogic>();

// =========================
// MANUFACTURE
// =========================
builder.Services.AddTransient<IOvipManufactureRepository, OvipManufactureRepository>();
builder.Services.AddTransient<IOvipManufacturePartRepository, OvipManufacturePartRepository>();

builder.Services.AddScoped<IOvipManufactureLogic, OvipManufactureLogic>();
builder.Services.AddScoped<IOvipManufacturePartLogic, OvipManufacturePartLogic>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<OvipProductProfile>();
});

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OvipDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // <-- Swagger JSON
    app.UseSwaggerUI();     // <-- Swagger UI (böngészőben /swagger)
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
