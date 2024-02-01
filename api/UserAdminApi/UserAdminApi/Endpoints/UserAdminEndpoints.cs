using Microsoft.AspNetCore.Http.HttpResults;
using UserAdminApi.Services;

public static class UserAdminEndpoints
{
    public static void MapUserAdminEndpoints(this WebApplication app)
    {
        app.MapGet("api/users", GetAllUsers)
            .WithName(nameof(GetAllUsers))
            .WithSummary("Get all users")
            .WithDescription(
                "This endpoint retrieves all users by calling the GetAllUsersAsync method from the UserAdminService interface. It returns an Ok response with an array of UserDto objects."
            );
        app.MapPost("api/users", CreateUser)
            .WithName(nameof(CreateUser))
            .WithSummary("Create a new user")
            .WithDescription(
                "This endpoint creates a new user by calling the CreateUserAsync method from the UserAdminService interface. It takes a UserCreateParams object as a parameter and returns a CreatedAtRoute response with the newly created user."
            );
        app.MapPut("api/users/{id}", UpdateUser)
            .WithName(nameof(UpdateUser))
            .WithSummary("Update an existing user")
            .WithDescription(
                "This endpoint updates an existing user by calling the UpdateUserAsync method from the UserAdminService interface. It takes a Guid id and a UserCreateParams object as parameters. If the user is found and updated successfully, it returns an Ok response with the updated user. If the user is not found, it returns a NotFound response with an error message."
            );
        app.MapDelete("api/users/{id}", DeleteUser)
            .WithName(nameof(DeleteUser))
            .WithSummary("Delete an existing user")
            .WithDescription(
                "This endpoint deletes an existing user by calling the DeleteUserAsync method from the UserAdminService interface. It takes a Guid id as a parameter. If the user is found and deleted successfully, it returns an Ok response. If the user is not found, it returns a NotFound response with an error message."
            );
    }

    private static async Task<Ok<UserDto[]>> GetAllUsers(IUserAdminService userAdminService)
    {
        var users = await userAdminService.GetAllUsersAsync();
        return TypedResults.Ok(users);
    }

    private static async Task<CreatedAtRoute<UserDto>> CreateUser(
        IUserAdminService userAdminService,
        UserCreateParams userCreateParams
    )
    {
        var newUser = await userAdminService.CreateUserAsync(userCreateParams);
        return TypedResults.CreatedAtRoute<UserDto>(newUser, $"/users/{newUser.Id}");
    }

    private static async Task<Results<Ok<UserDto>, NotFound<string>>> UpdateUser(
        IUserAdminService userAdminService,
        Guid id,
        UserCreateParams userParams
    )
    {
        try
        {
            var updatedUser = await userAdminService.UpdateUserAsync(id, userParams);
            return TypedResults.Ok(updatedUser);
        }
        catch (UserNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }

    private static async Task<Results<Ok, NotFound<string>>> DeleteUser(
        IUserAdminService userAdminService,
        Guid id
    )
    {
        try
        {
            await userAdminService.DeleteUserAsync(id);
            return TypedResults.Ok();
        }
        catch (UserNotFoundException e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }
}
