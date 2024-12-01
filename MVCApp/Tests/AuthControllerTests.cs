using Contracts.Services;
using Entities.Exceptions;
using Entities.Models.DTOs.User;
using Entities.ServiceHelpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;
using Xunit;
using System;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new AuthController(_mockAuthService.Object, _mockUserService.Object);

        // Мокаем HttpContext для Response.Cookies
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenAuthorizedSuccessfully()
    {
        var dto = new UserAuthorizationDto { Email = "gamshik@example.com", Password = "correctpassword" };
        var mockJwt = new Jwt { Token = "mockToken", Expire = DateTime.UtcNow.AddHours(1) };
        var mockAuthResult = new AuthorizeResult { Token = mockJwt, UserId = Guid.NewGuid() };
        var userDto = new UserDto { Id = mockAuthResult.UserId, Email = dto.Email, FirstName = "Gleb", LastName = "Kosharov" };
        var role = "User";

        _mockAuthService.Setup(service => service.AuthorizeAsync(dto)).ReturnsAsync(mockAuthResult);
        _mockUserService.Setup(service => service.GetByIdAsync<UserDto>(mockAuthResult.UserId)).ReturnsAsync(userDto);
        _mockUserService.Setup(service => service.GetUserRoleById(mockAuthResult.UserId)).ReturnsAsync(role);

        var result = await _controller.Login(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userDto.Email, returnedUser.Email);
        Assert.Equal(role, returnedUser.Role);
    }

    [Fact]
    public async Task Register_ReturnsOk_WhenRegistrationSucceeds()
    {
        var dto = new UserRegistrationDto
        {
            FirstName = "Gleb",
            LastName = "Kosharov",
            UserName = "Gamshik",
            Email = "gamshik@example.com",
            Password = "password123"
        };

        _mockAuthService.Setup(service => service.RegisterAsync(dto, It.Is<string[]>(roles => roles.Contains("User"))))
            .ReturnsAsync(false);

        var result = await _controller.Register(dto);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
    {
        var dto = new UserRegistrationDto
        {
            FirstName = "Gleb",
            LastName = "Kosharov",
            UserName = "Gamshik",
            Email = "gamshik@example.com",
            Password = "password123"
        };

        _mockAuthService.Setup(service => service.RegisterAsync(dto, It.IsAny<string[]>()))
            .ReturnsAsync(false);

        var result = await _controller.Register(dto);

        Assert.IsType<BadRequestResult>(result);
    }
}
