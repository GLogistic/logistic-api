using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;

public class CargoControllerTests
{
    private readonly Mock<ICargoService> _mockCargoService;
    private readonly CargoController _controller;

    public CargoControllerTests()
    {
        _mockCargoService = new Mock<ICargoService>();
        _controller = new CargoController(_mockCargoService.Object);
    }

    [Fact]
    public void IndexReturnsViewWithCargosWhenDataExists()
    {
        // Arrange
        var cargos = new PagedList<CargoDto>(
            new List<CargoDto>
            {
                new CargoDto { Id = Guid.NewGuid(), Title = "Cargo 1", Weight = 100, RegistrationNumber = "A123" },
                new CargoDto { Id = Guid.NewGuid(), Title = "Cargo 2", Weight = 200, RegistrationNumber = "B123" }
            },
            1, 1, 10
        );

        _mockCargoService.Setup(service => service.GetByPage<CargoDto>(It.IsAny<PaginationQueryParameters>()))
                         .Returns(cargos);

        // Act
        var result = _controller.Index(new PaginationQueryParameters());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PagedList<CargoDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public void IndexReturnsNoContentWhenNoData()
    {
        // Arrange
        var cargos = new PagedList<CargoDto>(new List<CargoDto>(), 1, 1, 10);
        _mockCargoService.Setup(service => service.GetByPage<CargoDto>(It.IsAny<PaginationQueryParameters>()))
                         .Returns(cargos);

        // Act
        var result = _controller.Index(new PaginationQueryParameters());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CreateReturnsViewWithErrorWhenModelStateInvalid()
    {
        // Arrange
        var invalidDto = new CargoCreateDto
        {
            Title = "",
            Weight = -1,
            RegistrationNumber = ""
        };
        _controller.ModelState.AddModelError("Title", "Cargo Title is required.");

        // Act
        var result = await _controller.Create(invalidDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CargoCreateDto>(viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    [Fact]
    public async Task CreateRedirectsToIndexWhenModelStateValid()
    {
        // Arrange
        var validDto = new CargoCreateDto
        {
            Title = "Cargo 1",
            Weight = 100,
            RegistrationNumber = "A123"
        };

        _mockCargoService.Setup(service => service.CreateAsync<CargoCreateDto, CargoDto>(It.IsAny<CargoCreateDto>()))
                         .ReturnsAsync(new CargoDto { Id = Guid.NewGuid(), Title = "Cargo 1", Weight = 100, RegistrationNumber = "A123" });

        // Act
        var result = await _controller.Create(validDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal(1, redirectResult.RouteValues["page"]);
        Assert.Equal(10, redirectResult.RouteValues["pageSize"]);
    }

    [Fact]
    public async Task UpdateReturnsViewWithErrorWhenModelStateInvalid()
    {
        // Arrange
        var invalidDto = new CargoUpdateDto
        {
            Id = Guid.NewGuid(),
            Title = "",
            Weight = -1,
            RegistrationNumber = ""
        };
        _controller.ModelState.AddModelError("Title", "Cargo Title is required.");

        // Act
        var result = await _controller.Update(invalidDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CargoUpdateDto>(viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    [Fact]
    public async Task DeleteRedirectsToIndexWhenCargoIsDeleted()
    {
        // Arrange
        var deleteDto = new CargoDeleteDto { Id = Guid.NewGuid() };

        _mockCargoService.Setup(service => service.DeleteByIdAsync(deleteDto.Id))
                         .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(deleteDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal(1, redirectResult.RouteValues["page"]);
        Assert.Equal(10, redirectResult.RouteValues["pageSize"]);
    }
}
