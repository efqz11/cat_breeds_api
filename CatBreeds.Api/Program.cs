using CatBreeds.Api.DbModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding db connection
var connectionString = builder.Configuration.GetConnectionString("AppDbConnection");
builder.Services.AddDbContext<CatDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();


// seed to database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    CatDbSeeder.Initialize(services);
}

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



