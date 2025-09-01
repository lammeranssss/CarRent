using CarRental.API.Mapping;
using CarRental.BLL.Mapping;
using CarRental.DAL.DI;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDalDependencies(builder.Configuration);

builder.Services.AddAutoMapper(typeof(DalToBllProfile), typeof(ApiToBllProfile), typeof(BllToApiProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
