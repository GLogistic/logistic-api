using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin", "User")]
    [Route("settlements")]
    [ApiController]
    public class SettlementController : BaseController
    {
        private readonly ISettlementService _settlementService;

        public SettlementController(ISettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        [HttpGet("", Name = "settlements")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters, string? titleFilter)
        {
            var settlements = _settlementService.GetByPage<SettlementDto>(parameters);

            if (settlements == null || !settlements.Any())
                return NoContent();

            ViewBag.CurrentPage = settlements.MetaData.CurrentPage;
            ViewBag.PageSize = settlements.MetaData.PageSize;
            ViewBag.TotalSize = settlements.MetaData.TotalSize;

            ViewBag.TitleFilter = titleFilter ?? "";

            ViewBag.HaveNext = settlements.MetaData.HaveNext;
            ViewBag.HavePrev = settlements.MetaData.HavePrev;

            ViewBag.ControllerName = "Settlement";
            ViewBag.ViewActionName = "settlements";
            ViewBag.CreateActionName = "create-settlement-view";
            ViewBag.DeleteActionName = "delete-settlement";
            ViewBag.UpdateActionName = "update-settlement-view";

            return View(settlements.Where(s => s.Title.Contains(titleFilter ?? "")));
        }
        [HttpGet("create", Name = "create-settlement-view")]
        public IActionResult CreateView() => View();
        [HttpPost("create", Name = "create-settlement")]
        public async Task<IActionResult> Create([FromForm] SettlementCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View("CreateView", dto);

            await _settlementService.CreateAsync<SettlementCreateDto, SettlementDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete-settlement", Name = "delete-settlement")]
        public async Task<IActionResult> Delete([FromForm] SettlementDeleteDto dto)
        {
            await _settlementService.DeleteByIdAsync(dto.Id);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpGet("update", Name = "update-settlement-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var settlement = await _settlementService.GetByIdAsync<SettlementUpdateDto>(id);
            return View(settlement);
        }
        [HttpPost("update", Name = "update-settlement")]
        public async Task<IActionResult> Update([FromForm] SettlementUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _settlementService.UpdateAsync<SettlementUpdateDto, SettlementDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
