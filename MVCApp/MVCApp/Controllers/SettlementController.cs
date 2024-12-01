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

            string jsonString = JsonSerializer.Serialize(new PaginationResponseParams
            {
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
        [HttpGet("all", Name = "all-settlements")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult GetAll()
        {
            var settlements = _settlementService.GetAll<SettlementDto>();
            return Ok(settlements);
        }
        [HttpPost("", Name = "create-settlement")]
        public async Task<IActionResult> Create([FromBody] SettlementCreateDto dto)
        {
            var newSettlement = await _settlementService.CreateAsync<SettlementCreateDto, SettlementDto>(dto);
            return Ok(newSettlement);
        }
        [HttpPost("delete", Name = "delete-settlement")]
        public async Task<IActionResult> Delete([FromBody] SettlementDeleteDto dto)
        {
            await _settlementService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpPut("update", Name = "update-settlement")]
        public async Task<IActionResult> Update([FromBody] SettlementUpdateDto dto)
        {
            var settlement = await _settlementService.UpdateAsync<SettlementUpdateDto, SettlementDto>(dto);
            return Ok(settlement);
        }
    }
}
