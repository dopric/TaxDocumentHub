using Microsoft.EntityFrameworkCore;
using TaxDocumentHub.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine(connectionString);
    options.UseSqlite(connectionString);
});

var app = builder.Build();


builder.Services.AddControllers();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers(); ;

app.Run();

