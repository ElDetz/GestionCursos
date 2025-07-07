using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
*/

var isTest = builder.Environment.IsEnvironment("Testing");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (isTest)
    {
        options.UseInMemoryDatabase("TestDb");
    }
    else
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CORS");
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }  // Esto permite que WebApplicationFactory funcione
