using Microsoft.EntityFrameworkCore;
using UserAdminApi.DbModel;

namespace UserAdminApi.Services;

public record UserDto(Guid Id, string Name, string Email, Int64 Credits);
public record UserCreateParams(string Name, string Email, Int64 Credits);
public interface IUserAdminService
{
    Task<User> CreateUserAsync(UserCreateParams user);
    Task<User[]> GetAllUsersAsync();
    Task<User> UpdateUserAsync(UserDto user);
    Task DeleteUserAsync(Guid id);
}
public class UserAdminService : IUserAdminService
{
    private readonly UserAdminDbContext _dbContext;

    public UserAdminService(UserAdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with id {id} not found");
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> CreateUserAsync(UserCreateParams user)
    {
        var newUser = _dbContext.Users.Add(new User()
        { 
            Credits = user.Credits,
            Email = user.Email,
            Name = user.Name
        });
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

