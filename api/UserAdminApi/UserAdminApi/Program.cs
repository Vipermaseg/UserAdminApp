using Microsoft.EntityFrameworkCore;
using UserAdminApi.DbModel;

var builder = WebApplication.CreateBuilder(args);
//Add configuration
var databaseConfig = builder.Configuration.GetSection("Database");

//Add database
builder.Services.AddDbContext<UserAdminDbContext>(
    x => x.UseSqlite(databaseConfig["ConnectionString"])
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/users", () =>
{
    return Enumerable.Range(1, 5).Select(index =>
        new User
        (
            Name: $"User {index}",
            Email: $"",
            Credits: 0
        )
    ).ToArray();
})
.WithName("GetAllUsers")
.WithSummary("Gets all users")
.WithOpenApi();

app.Run();

internal record UserDto(string Name, string Email, Int64 Credits);
