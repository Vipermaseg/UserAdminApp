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
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateParamsValidator>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is migrated/created
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserAdminDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.MapUserAdminEndpoints();

app.Run();
