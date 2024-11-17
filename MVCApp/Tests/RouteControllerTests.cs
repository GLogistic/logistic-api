using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using MVCApp.Controllers;

namespace MVCApp.Tests.Controllers
{
    public class RouteControllerTests
    {
        private readonly Mock<IRouteService> _mockRouteService;
        private readonly Mock<ISettlementService> _mockSettlementService;
        private readonly RouteController _controller;

        public RouteControllerTests()
        {
            _mockRouteService = new Mock<IRouteService>();
            _mockSettlementService = new Mock<ISettlementService>();
            _controller = new RouteController(_mockRouteService.Object, _mockSettlementService.Object);
        }

        [Fact]
        public void IndexReturnsViewWithRoutesWhenRoutesExist()
        {
            // Arrange
            var routes = new PagedList<RouteDto>(
                new List<RouteDto>
                {
                    new RouteDto { Id = Guid.NewGuid(), Distance = 100 },
                    new RouteDto { Id = Guid.NewGuid(), Distance = 200 }
                }, 1, 1, 2);

            _mockRouteService.Setup(service => service.GetByPage<RouteDto>(It.IsAny<PaginationQueryParameters>()))
                             .Returns(routes);

            // Act
            var result = _controller.Index(new PaginationQueryParameters());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PagedList<RouteDto>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void CreateViewReturnsViewWithSettlements()
        {
            // Arrange
            var settlements = new List<SettlementDto>
            {
                new SettlementDto { Id = Guid.NewGuid(), Title = "Settlement 1" },
                new SettlementDto { Id = Guid.NewGuid(), Title = "Settlement 2" }
            };

            _mockSettlementService.Setup(service => service.GetAll<SettlementDto>())
                                  .Returns(settlements);

            // Act
            var result = _controller.CreateView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var selectList = Assert.IsAssignableFrom<SelectList>(viewResult.ViewData["Settlements"]);
            Assert.Equal(2, selectList.Count());
        }

        [Fact]
        public async Task CreateRedirectsToIndexWhenModelStateIsValid()
        {
            // Arrange
            var createDto = new RouteCreateDto
            {
                StartSettlementId = Guid.NewGuid(),
                EndSettlementId = Guid.NewGuid(),
                Distance = 100
            };

            _mockRouteService.Setup(service => service.CreateAsync<RouteCreateDto, RouteDto>(It.IsAny<RouteCreateDto>()))
                             .ReturnsAsync(new RouteDto { Id = Guid.NewGuid(), Distance = 100 });

            _mockSettlementService.Setup(service => service.GetAll<SettlementDto>())
                                  .Returns(new List<SettlementDto>());

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(1, redirectResult.RouteValues["page"]);
        }

        [Fact]
        public async Task CreateReturnsViewWithErrorsWhenModelStateIsInvalid()
        {
            // Arrange
            var createDto = new RouteCreateDto();

            _mockSettlementService.Setup(service => service.GetAll<SettlementDto>())
                                  .Returns(new List<SettlementDto>
                                  {
                                      new SettlementDto { Id = Guid.NewGuid(), Title = "Settlement 1" }
                                  });

            // Act
            _controller.ModelState.AddModelError("Title", "Required");
            var result = await _controller.Create(createDto);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsAssignableFrom<RouteCreateDto>(viewResult.Model);
            Assert.Equal(createDto, model);
        }

        [Fact]
        public async Task DeleteRedirectsToIndexWhenSuccessful()
        {
            // Arrange
            var deleteDto = new RouteDeleteDto { Id = Guid.NewGuid() };

            _mockRouteService.Setup(service => service.DeleteByIdAsync(deleteDto.Id))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(deleteDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task UpdateViewReturnsViewWithRouteWhenRouteExists()
        {
            // Arrange
            var routeId = Guid.NewGuid();
            var route = new RouteUpdateDto
            {
                Id = routeId,
                StartSettlementId = Guid.NewGuid(),
                EndSettlementId = Guid.NewGuid(),
                Distance = 100
            };

            _mockRouteService.Setup(service => service.GetByIdAsync<RouteUpdateDto>(routeId))
                             .ReturnsAsync(route);

            // Act
            var result = await _controller.UpdateView(routeId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RouteUpdateDto>(viewResult.Model);
            Assert.Equal(routeId, model.Id);
        }

        [Fact]
        public async Task UpdateRedirectsToIndexWhenModelStateIsValid()
        {
            // Arrange
            var updateDto = new RouteUpdateDto
            {
                Id = Guid.NewGuid(),
                StartSettlementId = Guid.NewGuid(),
                EndSettlementId = Guid.NewGuid(),
                Distance = 150
            };

            _mockRouteService.Setup(service => service.UpdateAsync<RouteUpdateDto, RouteDto>(updateDto))
                             .ReturnsAsync(new RouteDto { Id = updateDto.Id, Distance = updateDto.Distance });

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
