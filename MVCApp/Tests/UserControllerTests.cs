using Contracts.Services;
using Entities.Models.DTOs.User;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
    }

    [Fact]
    public void IndexShouldReturnNoContentWhenNoUsersFound()
    {
        // Arrange
        var users = new PagedList<UserDto>(
            new List<UserDto> { },
            1, 1, 10
        );

        _mockUserService.Setup(service => service.GetByPage<UserDto>(It.IsAny<PaginationQueryParameters>()))
                        .Returns(users);

        // Act
        var result = _controller.Index(new PaginationQueryParameters());

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task CreateShouldRedirectToIndexWhenModelIsValid()
    {
        // Arrange
        var dto = new UserRegistrationDto
        {
            FirstName = "Gleb",
            LastName = "Kosharov",
            UserName = "Gamshik",
            Email = "gamshik@example.com",
            Password = "password123"
        };

        _mockAuthService.Setup(service => service.RegisterAsync(It.IsAny<UserRegistrationDto>(), It.IsAny<string[]>()))
                        .ReturnsAsync(true);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public async Task CreateShouldReturnViewWhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new UserRegistrationDto
        {
            FirstName = "Gleb",
            LastName = "Kosharov",
            UserName = "Gamshik",
            Email = "gamshik@example.com",
            Password = ""
        };

        _controller.ModelState.AddModelError("Password", "Password is required.");

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("CreateView", viewResult.ViewName);
        Assert.Equal(dto, viewResult.Model);
    }

    [Fact]
    public async Task DeleteShouldRedirectToIndexWhenUserIsDeleted()
    {
        // Arrange
        var dto = new UserDeleteDto { Id = Guid.NewGuid() };

        _mockUserService.Setup(service => service.DeleteByIdAsync(It.IsAny<Guid>()))
                        .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(dto);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
        Assert.Equal(10, redirectToActionResult.RouteValues["pageSize"]);
    }

    [Fact]
    public async Task UpdateShouldReturnViewWhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new UserUpdateDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Gleb",
            LastName = "Kosharov",
            UserName = "Gamshik",
            Email = "gamshik@example.com",
            SecurityStamp = ""
        };

        _controller.ModelState.AddModelError("SecurityStamp", "Security Stamp is required.");

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("UpdateView", viewResult.ViewName);
        Assert.Equal(dto, viewResult.Model);
    }

    [Fact]
    public async Task UpdateShouldRedirectToIndexWhenModelIsValid()
    {
        // Arrange
        var dto = new UserUpdateDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Gleb",
            LastName = "Kosharov",
            UserName = "Gamshik",
            Email = "gamshik@example.com",
            SecurityStamp = "security_stamp"
        };

        _mockUserService.Setup(service => service.UpdateAsync<UserUpdateDto, UserDto>(It.IsAny<UserUpdateDto>()))
                        .ReturnsAsync(new UserDto
                        {
                            Id = dto.Id,
                            FirstName = dto.FirstName,
                            LastName = dto.LastName,
                            UserName = dto.UserName,
                            Email = dto.Email
                        });

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(1, redirectToActionResult.RouteValues["page"]);
        Assert.Equal(10, redirectToActionResult.RouteValues["pageSize"]);
    }
}