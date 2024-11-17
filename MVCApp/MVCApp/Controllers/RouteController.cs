using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;
using System.ComponentModel.DataAnnotations;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin", "User")]
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
            var routes = _routeService.GetByPage<RouteDto>(parameters);

            if (routes == null || !routes.Any())
                return NoContent();

            ViewBag.CurrentPage = routes.MetaData.CurrentPage;
            ViewBag.PageSize = routes.MetaData.PageSize;
            ViewBag.TotalSize = routes.MetaData.TotalSize;

            ViewBag.StartSettlementTitleFilter = startSettlementTitleFilter ?? "";

            ViewBag.HaveNext = routes.MetaData.HaveNext;
            ViewBag.HavePrev = routes.MetaData.HavePrev;

            ViewBag.ControllerName = "Route";
            ViewBag.ViewActionName = "routes";
            ViewBag.CreateActionName = "create-route-view";
            ViewBag.DeleteActionName = "delete-route";
            ViewBag.UpdateActionName = "update-route-view";

            return View(routes.Where(r => r.StartSettlement!.Title.Contains(startSettlementTitleFilter ?? "")));
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
        [HttpPost("delete-route", Name = "delete-route")]
        public async Task<IActionResult> Delete([FromForm] RouteDeleteDto dto)
        {
            await _routeService.DeleteByIdAsync(dto.Id);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
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
