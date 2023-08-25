using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexion base de datos railwey
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("vivaviajes-db"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
});

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.WithOrigins("https://localhost:4200");
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
