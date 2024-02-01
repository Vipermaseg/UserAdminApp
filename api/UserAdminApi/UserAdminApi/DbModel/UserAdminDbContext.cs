using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserAdminApi.DbModel;

public class UserAdminDbContext : DbContext
{
    public UserAdminDbContext(DbContextOptions<UserAdminDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
}

public class User
{
    public Guid Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public required string Email { get; set; }

    public Int64 Credits { get; set; }
}
