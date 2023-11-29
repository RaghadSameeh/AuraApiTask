using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Reposatories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configration for Database
builder.Services.AddDbContext<Context>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("sql"), b => b.MigrationsAssembly("AuraTask"));});

//register Services
builder.Services.AddScoped<IGenericReposatory<Product>, GenericReposatory<Product>>();

// ConfigureServices method in Startup.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularDev");
app.UseAuthorization();

app.MapControllers();

app.Run();
