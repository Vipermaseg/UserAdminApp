using Microsoft.EntityFrameworkCore;
using UserAdminApi.DbModel;

namespace UserAdminApi.Services;

public record UserDto(Guid Id, string Name, string Email, Int64 Credits);
public record UserCreateParams(string Name, string Email, Int64 Credits);
public interface IUserAdminService
{
    Task<User> CreateUserAsync(User user);
    Task<User[]> GetAllUsersAsync();
    Task<User> UpdateUserAsync(UserDto user);
}
public class UserAdminService : IUserAdminService
{
    private readonly UserAdminDbContext _dbContext;

    public UserAdminService(UserAdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var newUser = _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return newUser.Entity;
    }
    public async Task<User[]> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToArrayAsync();
    }

    public async Task<User> UpdateUserAsync(UserDto user)
    {
        var existingUser = await _dbContext.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with id {user.Id} not found");
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Credits = user.Credits;

        await _dbContext.SaveChangesAsync();
        return existingUser;
    }
}

