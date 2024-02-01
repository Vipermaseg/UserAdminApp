using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace UserAdminApi.DbModel;
public class UserAdminDbContext : DbContext
{
    public UserAdminDbContext(DbContextOptions<UserAdminDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Int64 Credits { get; set; }
}
