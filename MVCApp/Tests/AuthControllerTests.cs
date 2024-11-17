using Contracts.Services;
using Entities;
using Entities.Exceptions;
using Entities.Models.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
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
    public async Task LoginReturnsRedirectToActionWhenSuccessful()
    {
        // Arrange
        var dto = new UserAuthorizationDto { Email = "gamshik@example.com", Password = "password123" };
        var token = new Jwt { Token = "fake_token", Expire = DateTime.UtcNow.AddHours(1) };

        _mockAuthService
            .Setup(service => service.AuthorizeAsync(dto))
            .ReturnsAsync(token);

        // Mock the HttpContext and Response
        var mockHttpContext = new Mock<HttpContext>();
        var mockResponse = new Mock<HttpResponse>();
        var mockCookieCollection = new Mock<IResponseCookies>();

        mockHttpContext.SetupGet(context => context.Response).Returns(mockResponse.Object);
        mockResponse.SetupGet(response => response.Cookies).Returns(mockCookieCollection.Object);

        // Set the HttpContext for the controller
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };


        // Act
        var result = await _controller.Login(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
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
