using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Controllers;

namespace Tests
{
    public class SettlementControllerTests
    {
        private readonly Mock<ISettlementService> _mockSettlementService;
        private readonly SettlementController _controller;

        public SettlementControllerTests()
        {
            _mockSettlementService = new Mock<ISettlementService>();
            _controller = new SettlementController(_mockSettlementService.Object);
        }

        [Fact]
        public void IndexReturnsViewWithSettlements()
        {
            // Arrange
            var settlements = new PagedList<SettlementDto>
            (
                new List<SettlementDto>
                {
                    new SettlementDto { Id = Guid.NewGuid(), Title = "Test Settlement 1" },
                    new SettlementDto { Id = Guid.NewGuid(), Title = "Test Settlement 2" }
                },
                5,
                1,
                10
            );
            _mockSettlementService
                .Setup(service => service.GetByPage<SettlementDto>(It.IsAny<PaginationQueryParameters>()))
                .Returns(settlements);

            // Act
            var result = _controller.Index(new PaginationQueryParameters());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PagedList<SettlementDto>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task CreatePostReturnsRedirectWhenValidModel()
        {
            // Arrange
            var dto = new SettlementCreateDto { Title = "New Settlement" };
            _mockSettlementService.Setup(service => service.CreateAsync<SettlementCreateDto, SettlementDto>(It.IsAny<SettlementCreateDto>()))
                .ReturnsAsync(new SettlementDto { Id = Guid.NewGuid(), Title = "New Settlement" });

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task DeletePostReturnsRedirect()
        {
            // Arrange
            var dto = new SettlementDeleteDto { Id = Guid.NewGuid() };
            _mockSettlementService.Setup(service => service.DeleteByIdAsync(dto.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(dto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task UpdateViewReturnsViewWithSettlement()
        {
            // Arrange
            var settlementDto = new SettlementUpdateDto { Id = Guid.NewGuid(), Title = "Test Settlement" };
            _mockSettlementService.Setup(service => service.GetByIdAsync<SettlementUpdateDto>(It.IsAny<Guid>()))
                .ReturnsAsync(settlementDto);

            // Act
            var result = await _controller.UpdateView(Guid.NewGuid());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SettlementUpdateDto>(viewResult.Model);
            Assert.Equal("Test Settlement", model.Title);
        }

        [Fact]
        public async Task UpdatePostReturnsRedirectWhenValidModel()
        {
            // Arrange
            var dto = new SettlementUpdateDto { Id = Guid.NewGuid(), Title = "Updated Settlement" };
            _mockSettlementService.Setup(service => service.UpdateAsync<SettlementUpdateDto, SettlementDto>(It.IsAny<SettlementUpdateDto>()))
                .ReturnsAsync(new SettlementDto { Id = dto.Id, Title = "Updated Settlement" });

            // Act
            var result = await _controller.Update(dto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}