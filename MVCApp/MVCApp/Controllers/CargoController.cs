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
        [HttpGet("create", Name = "create-cargo-view")]
        public IActionResult CreateView() => View();
        [HttpPost("", Name = "create-cargo")]
        public async Task<IActionResult> Create([FromForm] CargoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View("CreateView", dto);

            await _cargoService.CreateAsync<CargoCreateDto, CargoDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete", Name = "delete-cargo")]
        public async Task<IActionResult> Delete([FromBody] CargoDeleteDto dto)
        {
            await _cargoService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpGet("update", Name = "update-cargo-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var cargo = await _cargoService.GetByIdAsync<CargoUpdateDto>(id);
            return View(cargo);
        }
        [HttpPost("update", Name = "update-cargo")]
        public async Task<IActionResult> Update([FromForm] CargoUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _cargoService.UpdateAsync<CargoUpdateDto, CargoDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
