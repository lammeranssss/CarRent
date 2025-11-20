using CarRental.API.DI;
using CarRental.DAL.DataContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

await CarRentalDbContext.ApplyMigrationsAsync(app.Services);

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
