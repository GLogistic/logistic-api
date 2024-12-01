using Contracts.Services;
using Entities;
using Entities.Models.DTOs;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;
using System.Text.Json;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles]
    [Route("cargo")]
    [ApiController]
    public class CargoController : BaseController
    {
        private readonly ICargoService _cargoService;

        public CargoController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        [HttpGet("", Name = "cargos")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters, string? titleFilter)
        {
            var cargos = _cargoService.GetByPage<CargoDto>(parameters, titleFilter);

            if (cargos == null || !cargos.Any())
                return NoContent();

            string jsonString = JsonSerializer.Serialize(new PaginationResponseParams
            {
                currentPage = cargos.MetaData.CurrentPage,
                pageSize = cargos.MetaData.PageSize,
                totalSize = cargos.MetaData.TotalSize,
                totalPages = cargos.MetaData.TotalPages,
                haveNext = cargos.MetaData.HaveNext,
                havePrev = cargos.MetaData.HavePrev,
            });

            HttpContext.Response.Headers.Add("X-Pagination-Params", jsonString);

            return Ok(cargos);
        }
        [HttpPost("", Name = "create-cargo")]
        public async Task<IActionResult> Create([FromBody] CargoCreateDto dto)
        {
            var newCargo = await _cargoService.CreateAsync<CargoCreateDto, CargoDto>(dto);
            return Ok(newCargo);
        }
        [HttpPost("delete", Name = "delete-cargo")]
        public async Task<IActionResult> Delete([FromBody] CargoDeleteDto dto)
        {
            await _cargoService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpPut("update", Name = "update-cargo")]
        public async Task<IActionResult> Update([FromBody] CargoUpdateDto dto)
        {
            var cargo = await _cargoService.UpdateAsync<CargoUpdateDto, CargoDto>(dto);
            return Ok(cargo);
        }
    }
}
