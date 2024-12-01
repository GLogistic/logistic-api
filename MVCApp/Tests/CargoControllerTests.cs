using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Http;
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

        // Мокаем HttpContext для Response.Headers
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public void Index_ReturnsOk_WhenCargosExist()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var cargos = new PagedList<CargoDto>(new List<CargoDto>
        {
            new CargoDto { Title = "Cargo1", Weight = 100, RegistrationNumber = "ABC123" },
            new CargoDto { Title = "Cargo2", Weight = 200, RegistrationNumber = "XYZ789" }
        }, 1, 10, 2);

        _mockCargoService.Setup(s => s.GetByPage<CargoDto>(paginationParams, null))
            .Returns(cargos);

        // Act
        var result = _controller.Index(paginationParams, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCargos = Assert.IsType<PagedList<CargoDto>>(okResult.Value);
        Assert.Equal(2, returnedCargos.Count);
    }

    [Fact]
    public void Index_ReturnsNoContent_WhenNoCargosExist()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        _mockCargoService.Setup(s => s.GetByPage<CargoDto>(paginationParams, null))
            .Returns(new PagedList<CargoDto>(new List<CargoDto>(), 1, 10, 0));

        // Act
        var result = _controller.Index(paginationParams, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenCargoIsCreated()
    {
        // Arrange
        var createDto = new CargoCreateDto { Title = "New Cargo", Weight = 150, RegistrationNumber = "REG123" };
        var createdCargo = new CargoDto { Title = "New Cargo", Weight = 150, RegistrationNumber = "REG123" };

        _mockCargoService.Setup(s => s.CreateAsync<CargoCreateDto, CargoDto>(createDto))
            .ReturnsAsync(createdCargo);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCargo = Assert.IsType<CargoDto>(okResult.Value);
        Assert.Equal(createDto.Title, returnedCargo.Title);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCargoIsDeleted()
    {
        // Arrange
        var deleteDto = new CargoDeleteDto { Id = Guid.NewGuid() };
        _mockCargoService.Setup(s => s.DeleteByIdAsync(deleteDto.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(deleteDto);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenCargoIsUpdated()
    {
        // Arrange
        var updateDto = new CargoUpdateDto { Id = Guid.NewGuid(), Title = "Updated Cargo", Weight = 200, RegistrationNumber = "UPDATED123" };
        var updatedCargo = new CargoDto { Title = "Updated Cargo", Weight = 200, RegistrationNumber = "UPDATED123" };

        _mockCargoService.Setup(s => s.UpdateAsync<CargoUpdateDto, CargoDto>(updateDto))
            .ReturnsAsync(updatedCargo);

        // Act
        var result = await _controller.Update(updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCargo = Assert.IsType<CargoDto>(okResult.Value);
        Assert.Equal(updateDto.Title, returnedCargo.Title);
    }
}
