using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using team1_fe_gc_proyecto_final_backend;
using team1_fe_gc_proyecto_final_backend.Data;
using team1_fe_gc_proyecto_final_backend.Models;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
builder.WebHost.UseUrls($"http://*:{port};");

var app = builder.Build();

startup.Configure(app, app.Environment);

app.MapControllers();

app.Run();
