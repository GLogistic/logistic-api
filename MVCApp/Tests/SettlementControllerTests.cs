using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using MVCApp.Controllers;

public class SettlementControllerTests
{
    private readonly Mock<ISettlementService> _mockSettlementService;
    private readonly SettlementController _controller;

    public SettlementControllerTests()
    {
        _mockSettlementService = new Mock<ISettlementService>();
        _controller = new SettlementController(_mockSettlementService.Object);

        // Мокаем HttpContext для Response.Headers
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public void Index_ReturnsOk_WhenSettlementsExist()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        var settlements = new PagedList<SettlementDto>(new List<SettlementDto>
        {
            new SettlementDto { Title = "Settlement1" },
            new SettlementDto { Title = "Settlement2" }
        }, 1, 10, 2);

        _mockSettlementService.Setup(s => s.GetByPage<SettlementDto>(paginationParams, null))
            .Returns(settlements);

        // Act
        var result = _controller.Index(paginationParams, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSettlements = Assert.IsType<PagedList<SettlementDto>>(okResult.Value);
        Assert.Equal(2, returnedSettlements.Count);
    }

    [Fact]
    public void Index_ReturnsNoContent_WhenNoSettlementsExist()
    {
        // Arrange
        var paginationParams = new PaginationQueryParameters { page = 1, pageSize = 10 };
        _mockSettlementService.Setup(s => s.GetByPage<SettlementDto>(paginationParams, null))
            .Returns(new PagedList<SettlementDto>(new List<SettlementDto>(), 1, 10, 0));

        // Act
        var result = _controller.Index(paginationParams, null);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void GetAll_ReturnsOk_WithSettlements()
    {
        // Arrange
        var settlements = new List<SettlementDto>
        {
            new SettlementDto { Title = "Settlement1" },
            new SettlementDto { Title = "Settlement2" }
        };

        _mockSettlementService.Setup(s => s.GetAll<SettlementDto>())
            .Returns(settlements);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSettlements = Assert.IsType<List<SettlementDto>>(okResult.Value);
        Assert.Equal(2, returnedSettlements.Count);
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenSettlementIsCreated()
    {
        // Arrange
        var createDto = new SettlementCreateDto { Title = "New Settlement" };
        var createdSettlement = new SettlementDto { Title = "New Settlement" };

        _mockSettlementService.Setup(s => s.CreateAsync<SettlementCreateDto, SettlementDto>(createDto))
            .ReturnsAsync(createdSettlement);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSettlement = Assert.IsType<SettlementDto>(okResult.Value);
        Assert.Equal(createDto.Title, returnedSettlement.Title);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSettlementIsDeleted()
    {
        // Arrange
        var deleteDto = new SettlementDeleteDto { Id = Guid.NewGuid() };
        _mockSettlementService.Setup(s => s.DeleteByIdAsync(deleteDto.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(deleteDto);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenSettlementIsUpdated()
    {
        // Arrange
        var updateDto = new SettlementUpdateDto { Id = Guid.NewGuid(), Title = "Updated Settlement" };
        var updatedSettlement = new SettlementDto { Title = "Updated Settlement" };

        _mockSettlementService.Setup(s => s.UpdateAsync<SettlementUpdateDto, SettlementDto>(updateDto))
            .ReturnsAsync(updatedSettlement);

        // Act
        var result = await _controller.Update(updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedSettlement = Assert.IsType<SettlementDto>(okResult.Value);
        Assert.Equal(updateDto.Title, returnedSettlement.Title);
    }
}
