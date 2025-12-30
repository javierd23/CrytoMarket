using CryptoMarket.Application.Interfaces;
using CryptoMarket.Application.Services;
using CryptoMarket.Infrastructure.Binance;
using CryptoMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IBinanceClient, BinanceClient>(client =>
{
    client.BaseAddress = new Uri("https://api.binance.com");
});
builder.Services.AddScoped<ISymbolService, SymbolService>();
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<ICandleService, CandleService>();



var app = builder.Build();

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
