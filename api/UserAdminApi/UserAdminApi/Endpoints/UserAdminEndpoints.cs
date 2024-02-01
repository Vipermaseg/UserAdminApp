using Microsoft.AspNetCore.Mvc;
using UserAdminApi.Services;

public static class UserAdminEndpoints
{
    public static void MapUserAdminEndpoints(this WebApplication app)
    {
        app.MapGet("api/users", GetAllUsers).WithName(nameof(GetAllUsers));
        app.MapPost("api/users", CreateUser).WithName(nameof(CreateUser));
        app.MapPut("api/users/{id}", UpdateUser).WithName(nameof(UpdateUser));
        app.MapDelete("api/users/{id}", DeleteUser).WithName(nameof(DeleteUser));
    }

    private static async Task<IResult> GetAllUsers(IUserAdminService userAdminService)
    {
        var users = await userAdminService.GetAllUsersAsync();
        return Results.Ok(users);
    }

    private static async Task<IResult> CreateUser(
        IUserAdminService userAdminService,
        UserCreateParams userCreateParams
    )
    {
        var newUser = await userAdminService.CreateUserAsync(userCreateParams);
        return Results.Created($"/users/{newUser.Id}", newUser);
    }

    private static async Task<IResult> UpdateUser(
        IUserAdminService userAdminService,
        Guid id,
        UserDto userDto
    )
    {
        if (id != userDto.Id)
        {
            return Results.BadRequest("Id in path does not match id in body");
        }
        var updatedUser = await userAdminService.UpdateUserAsync(userDto);
        return Results.Ok(updatedUser);
    }

    private static async Task<IResult> DeleteUser(IUserAdminService userAdminService, Guid id)
    {
        await userAdminService.DeleteUserAsync(id);
        return Results.Ok();
    }
}
