using Contracts.Services;
using Entities;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ISettlementService _settlementService;

        public RouteController(IRouteService routeService, ISettlementService settlementService)
        {
            _routeService = routeService;
            _settlementService = settlementService;
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
        [HttpGet("create", Name = "create-route-view")]
        public IActionResult CreateView()
        {
            var settlements = _settlementService.GetAll<SettlementDto>();
            ViewBag.Settlements = new SelectList(settlements, "Id", "Title");
            return View();
        }
        [HttpPost("", Name = "create-route")]
        public async Task<IActionResult> Create([FromForm] RouteCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var settlements = _settlementService.GetAll<SettlementDto>();
                ViewBag.Settlements = new SelectList(settlements, "Id", "Title");
                return View(dto);
            }

            var validationResult = dto.ValidateSettlements();
            if (validationResult != ValidationResult.Success)
            {
                ModelState.AddModelError("", validationResult.ErrorMessage!);
                var settlements = _settlementService.GetAll<SettlementDto>();
                ViewBag.Settlements = new SelectList(settlements, "Id", "Title");
                return View("CreateView", dto);
            }


            await _routeService.CreateAsync<RouteCreateDto, RouteDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete", Name = "delete-route")]
        public async Task<IActionResult> Delete([FromBody] RouteDeleteDto dto)
        {
            await _routeService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpGet("update", Name = "update-route-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var settlements = _settlementService.GetAll<SettlementDto>();
            ViewBag.Settlements = new SelectList(settlements, "Id", "Title");

            var route = await _routeService.GetByIdAsync<RouteUpdateDto>(id);
            return View(route);
        }
        [HttpPost("update", Name = "update-route")]
        public async Task<IActionResult> Update([FromForm] RouteUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _routeService.UpdateAsync<RouteUpdateDto, RouteDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
