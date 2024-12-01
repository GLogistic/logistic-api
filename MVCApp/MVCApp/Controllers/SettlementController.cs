using Contracts.Services;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;
using System.Text.Json;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles]
    [Route("settlement")]
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
            var settlements = _settlementService.GetByPage<SettlementDto>(parameters, titleFilter);

            if (settlements == null || !settlements.Any())
                return NoContent();

            string jsonString = JsonSerializer.Serialize(new PaginationResponseParams    { 
                currentPage = settlements.MetaData.CurrentPage,
                pageSize = settlements.MetaData.PageSize,
                totalSize = settlements.MetaData.TotalSize,
                totalPages = settlements.MetaData.TotalPages,
                haveNext = settlements.MetaData.HaveNext,
                havePrev = settlements.MetaData.HavePrev,
            });

            HttpContext.Response.Headers.Add("X-Pagination-Params", jsonString);

            return Ok(settlements);
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
        [HttpPost("delete", Name = "delete-settlement")]
        public async Task<IActionResult> Delete([FromBody] SettlementDeleteDto dto)
        {
            await _settlementService.DeleteByIdAsync(dto.Id);
            return Ok();
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
