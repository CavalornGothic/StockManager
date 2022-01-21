using Microsoft.EntityFrameworkCore;
using StockManager.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StockManagerDbContext>(options => options.UseSqlServer(@""));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

var database = app.Services.CreateScope().ServiceProvider.GetService<StockManagerDbContext>();
database.Database.EnsureCreated();

app.Run();
