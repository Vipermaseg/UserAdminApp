using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using UserAdminApi.DbModel;
using UserAdminApi.Services;

namespace TestUserAdminApi;

public class UserAdminServiceTests
{
    private Mock<UserAdminDbContext> _mockDbContext;
    private UserAdminService _service;
    private List<User> _testUserList;
    private User _testUser;

    public UserAdminServiceTests()
    {
        _mockDbContext = new Mock<UserAdminDbContext>();
        _service = new UserAdminService(_mockDbContext.Object);

        _testUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "testuser@example.com",
            Credits = 100
        };

        _testUserList =
        [
            new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Email = "john@example.com",
                Credits = 100
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Jane",
                Email = "jane@example.com",
                Credits = 200
            }
        ];
    }

    [Fact]
    public async Task DeleteUserAsync_DeletesUser()
    {
        // Arrange
        var users = new List<User> { _testUser }.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _mockDbContext.Setup(db => db.Users).Returns(mockSet.Object);
        _mockDbContext.Setup(db => db.Users.FindAsync(_testUser.Id)).ReturnsAsync(_testUser);

        // Act
        await _service.DeleteUserAsync(_testUser.Id);

        // Assert
        mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Once());
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once());
    }

    [Fact]
    public async Task DeleteUserAsync_NonExistingUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var users = new List<User> { _testUser }.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _mockDbContext.Setup(db => db.Users).Returns(mockSet.Object);

        // Act and Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _service.DeleteUserAsync(_testUser.Id));
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = _testUserList.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();

        mockSet.As<IAsyncEnumerable<User>>()
            .Setup(m => m.GetAsyncEnumerator(new CancellationToken()))
            .Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

        mockSet.As<IQueryable<User>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<User>(users.Provider));

        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _mockDbContext.Setup(db => db.Users).Returns(mockSet.Object);

        // Act
        var result = await _service.GetAllUsersAsync();

        // Assert
        Assert.Equal(_testUserList.Count, result.Length);
        Assert.Equal(_testUserList[0].Id, result[0].Id);
        Assert.Equal(_testUserList[0].Name, result[0].Name);
        Assert.Equal(_testUserList[0].Email, result[0].Email);
        Assert.Equal(_testUserList[0].Credits, result[0].Credits);
        Assert.Equal(_testUserList[1].Id, result[1].Id);
        Assert.Equal(_testUserList[1].Name, result[1].Name);
        Assert.Equal(_testUserList[1].Email, result[1].Email);
        Assert.Equal(_testUserList[1].Credits, result[1].Credits);
    }

    [Fact]
    public async Task UpdateUserAsync_ExistingUser_ReturnsUpdatedUser()
    {
        // Arrange
        var users = new List<User> { _testUser }.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _mockDbContext.Setup(db => db.Users).Returns(mockSet.Object);
        var userParams = new UserCreateParams("Updated User", "updateduser@email.com", 200);
        _mockDbContext.Setup(db => db.Users.FindAsync(_testUser.Id)).ReturnsAsync(_testUser);

        // Act
        var result = await _service.UpdateUserAsync(_testUser.Id, userParams);


        // Assert
        Assert.Equal(_testUser.Id, result.Id);
        Assert.Equal(_testUser.Name, result.Name);
        Assert.Equal(_testUser.Email, result.Email);
        Assert.Equal(_testUser.Credits, result.Credits);
    }

    [Fact]
    public async Task UpdateUserAsync_NonExistingUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var users = new List<User> { _testUser }.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _mockDbContext.Setup(db => db.Users).Returns(mockSet.Object);
        _mockDbContext.Setup(db => db.Users.FindAsync(_testUser.Id)).ReturnsAsync(_testUser);
        var userParams = new UserCreateParams("Updated User", "updateduser@email.com", 200);

        // Act and Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _service.UpdateUserAsync(Guid.NewGuid(), userParams)
        );
    }
}
