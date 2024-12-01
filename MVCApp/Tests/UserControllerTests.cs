using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Contracts.Services;
using Entities.Models.DTOs.User;
using Entities.Pagination;
using MVCApp.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockAuthService = new Mock<IAuthService>();
        _controller = new UserController(_mockUserService.Object, _mockAuthService.Object);

        // Mock HttpContext for Response.Headers
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public void Index_ReturnsOk_WhenUsersExist()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var users = new PagedList<UserDto>(new List<UserDto>
        {
            new UserDto { FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new UserDto { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" }
        }, 1, 10, 2);

        _mockUserService.Setup(s => s.GetByPage<UserDto>(paginationParams)).Returns(users);

        // Act
        var result = _controller.Index(paginationParams);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsType<PagedList<UserDto>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
    }

    [Fact]
    public void Index_ReturnsNoContent_WhenNoUsersExist()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        _mockUserService.Setup(s => s.GetByPage<UserDto>(paginationParams))
            .Returns(new PagedList<UserDto>(new List<UserDto>(), 1, 10, 0));

        // Act
        var result = _controller.Index(paginationParams);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenUserIsDeleted()
    {
        // Arrange
        var deleteDto = new UserDeleteDto { Id = Guid.NewGuid() };
        _mockUserService.Setup(s => s.DeleteByIdAsync(deleteDto.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(deleteDto);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenUserIsUpdated()
    {
        // Arrange
        var updateDto = new UserUpdateDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Updated",
            LastName = "User",
            UserName = "updateduser",
            Email = "updated@example.com",
            SecurityStamp = "newstamp"
        };

        var updatedUser = new UserDto
        {
            FirstName = "Updated",
            LastName = "User",
            UserName = "updateduser",
            Email = "updated@example.com"
        };

        _mockUserService.Setup(s => s.UpdateAsync<UserUpdateDto, UserDto>(updateDto))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.Update(updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(updateDto.Email, returnedUser.Email);
    }
}
