using Microsoft.EntityFrameworkCore;
using RedisExchangeApi.API.Context;
using RedisExchangeApi.API.Entities;
using RedisExchangeApi.API.Infrastracture;
using RedisExchangeApi.API.Repositories;
using RedisExchangeApi.API.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI Kaydý
builder.Services.AddScoped<IProductService, ProductService>();
// Önce temel repository'yi ekle
builder.Services.AddScoped<ProductRepository>();
// Sonra decorator olan redis cache'li versiyonu ekle
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var redis = sp.GetRequiredService<IDatabase>();
    var innerRepo = sp.GetRequiredService<ProductRepository>();
    return new ProductRepositoryWithRedis(redis, innerRepo);
});
builder.Services.AddSingleton<RedisService>();
builder.Services.AddSingleton<IDatabase>(sp =>
{
    var redisService = sp.GetRequiredService<RedisService>();
    return redisService.GetDb(0);
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("MyInMemoryDb");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Redis'e baðlantý burada baþlatýlýr
var redisService = app.Services.GetRequiredService<RedisService>();
redisService.Connect();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
