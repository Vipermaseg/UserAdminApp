using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserAdminApi.DbModel;
using UserAdminApi.Services;
using UserAdminApi.Validators;

var builder = WebApplication.CreateBuilder(args);
//Add configuration
var databaseConfig = builder.Configuration.GetSection("Database");

//Add database
builder.Services.AddDbContext<UserAdminDbContext>(
    x => x.UseSqlite(databaseConfig["ConnectionString"])
);

//Add services 
builder.Services.AddScoped<IUserAdminService, UserAdminService>();

//Add validators
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateParamsValidator>();

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


/*app.MapGet("/users", () =>
{
    return Enumerable.Range(1, 5).Select(index =>
        new UserDto
        (
            Id: Guid.NewGuid(),
            Name: $"User {index}",
            Email: $"",
            Credits: 0
        )
    ).ToArray();
})
.WithName("GetAllUsers")
.WithSummary("Gets all users")
.WithOpenApi();

app.MapPost("/users", async (UserCreateParams userParams, IUserAdminService service) =>
{
    var user = new User
    {
        Id = Guid.NewGuid(),
        Name = userParams.Name,
        Email = userParams.Email,
        Credits = userParams.Credits
    };
    return await service.CreateUserAsync(user);
})
.AddEndpointFilter<ValidationFilter<UserCreateParams>>()
.WithName("AddUser")
.WithSummary("Create user")
.WithOpenApi();*/

app.MapUserAdminEndpoints();

app.Run();
