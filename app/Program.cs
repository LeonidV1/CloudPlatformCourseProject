using Microsoft.EntityFrameworkCore;
using rut_shop.net.api;
using rut_shop.net.database;
using rut_shop.net.dto;
using rut_shop.net.interfaces;
using rut_shop.net.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Cloud Computing Platform API",
        Version = "v1",
        Description = "Демонстрационный API платформы облачных вычислений: пакеты, компании, подписки, счета."
    });
});

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Не найдена строка подключения ConnectionStrings:Postgres.");

builder.Services.AddDbContext<CloudPlatformDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IComputingPackageService, ComputingPackageService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IMapper, Mapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<CloudPlatformDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cloud Computing Platform API v1");
        options.RoutePrefix = "swagger";
    });
}

var api = app.MapGroup("/api");
api.MapComputingPackagesEndpoints();
api.MapCompaniesEndpoints();
api.MapSubscriptionsEndpoints();

app.MapGet("/", () => Results.Ok(new
{
    message = "Cloud Computing Platform API работает. Откройте /api/packages, /api/companies, /api/subscriptions."
}));

await app.RunAsync();
