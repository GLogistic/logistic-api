using Contracts.Services;
using Entities.Exceptions;
using Entities.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object, _mockUserService.Object);
    }

    [Fact]
    public void LoginViewReturnsViewResult()
    {
        // Act
        var result = _controller.LoginView();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public async Task LoginReturnsRedirectToLoginViewWhenNotFoundException()
    {
        // Arrange
        var dto = new UserAuthorizationDto { Email = "gamshik@example.com", Password = "wrongpassword" };

        _mockAuthService
            .Setup(service => service.AuthorizeAsync(dto))
            .ThrowsAsync(new NotFoundException("User not found"));

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("LoginView", redirectResult.ActionName);
    }

    [Fact]
    public void RegisterViewReturnsViewResult()
    {
        // Act
        var result = _controller.RegisterView();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task RegisterReturnsRedirectToRegisterViewWhenFailed()
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

        _mockAuthService
            .Setup(service => service.RegisterAsync(dto, It.IsAny<string[]>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RegisterView", redirectResult.ActionName);
    }
}
