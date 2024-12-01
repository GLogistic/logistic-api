using Contracts.Services;
using Entities;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles]
    [Route("route")]
    [ApiController]
    public class RouteController : BaseController
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("", Name = "routes")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters, string? startSettlementTitleFilter)
        {
            var routes = _routeService.GetByPage<RouteDto>(parameters, startSettlementTitleFilter);

            if (routes == null || !routes.Any())
                return NoContent();

            string jsonString = JsonSerializer.Serialize(new PaginationResponseParams
            {
                currentPage = routes.MetaData.CurrentPage,
                pageSize = routes.MetaData.PageSize,
                totalSize = routes.MetaData.TotalSize,
                totalPages = routes.MetaData.TotalPages,
                haveNext = routes.MetaData.HaveNext,
                havePrev = routes.MetaData.HavePrev,
            });

            HttpContext.Response.Headers.Add("X-Pagination-Params", jsonString);

            return Ok(routes);
        }
        [HttpPost("", Name = "create-route")]
        public async Task<IActionResult> Create([FromBody] RouteCreateDto dto)
        {
            var validationResult = dto.ValidateSettlements();
            if (validationResult != ValidationResult.Success)
                return BadRequest();

            await _routeService.CreateAsync<RouteCreateDto, RouteDto>(dto);
            return Ok();
        }
        [HttpPost("delete", Name = "delete-route")]
        public async Task<IActionResult> Delete([FromBody] RouteDeleteDto dto)
        {
            await _routeService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpPut("update", Name = "update-route")]
        public async Task<IActionResult> Update([FromBody] RouteUpdateDto dto)
        {
            var route = await _routeService.UpdateAsync<RouteUpdateDto, RouteDto>(dto);
            return Ok(route);
        }
    }
}
