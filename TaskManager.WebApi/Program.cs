using Microsoft.OpenApi.Models;
using TaskManager.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( opt =>
    opt.MapType<DateOnly>(() => new OpenApiSchema {
        Type = "string",
        Format = "date"
    })
);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

var app = builder.Build();

DatabaseCreator.Create();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();