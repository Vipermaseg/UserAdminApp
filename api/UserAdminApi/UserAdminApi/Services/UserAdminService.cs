using Microsoft.EntityFrameworkCore;
using UserAdminApi.DbModel;

namespace UserAdminApi.Services;

public record UserDto(Guid Id, string Name, string Email, Int64 Credits);

public record UserCreateParams(string Name, string Email, Int64 Credits);

public interface IUserAdminService
{
    Task<UserDto> CreateUserAsync(UserCreateParams user);
    Task<UserDto[]> GetAllUsersAsync();
    Task<UserDto> UpdateUserAsync(Guid id, UserCreateParams user);
    Task DeleteUserAsync(Guid id);
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id)
        : base($"User with id {id} not found") { }
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
            throw new UserNotFoundException(id);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserDto> CreateUserAsync(UserCreateParams user)
    {
        var newUser = _dbContext.Users.Add(
            new User()
            {
                Credits = user.Credits,
                Email = user.Email,
                Name = user.Name
            }
        );
        await _dbContext.SaveChangesAsync();
        return new UserDto(
            newUser.Entity.Id,
            newUser.Entity.Name,
            newUser.Entity.Email,
            newUser.Entity.Credits
        );
    }

    public async Task<UserDto[]> GetAllUsersAsync()
    {
        return await _dbContext.Users
            .Select(u => new UserDto(u.Id, u.Name, u.Email, u.Credits))
            .ToArrayAsync();
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UserCreateParams user)
    {
        var existingUser = await _dbContext.Users.FindAsync(id);
        if (existingUser == null)
            throw new UserNotFoundException(id);

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Credits = user.Credits;

        await _dbContext.SaveChangesAsync();
        return new UserDto(
            existingUser.Id,
            existingUser.Name,
            existingUser.Email,
            existingUser.Credits
        );
    }
}
