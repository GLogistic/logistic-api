using Contracts.Services;
using Entities.Models.DTOs.User;
using Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;
using System.Text.Json;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin")]
    [Route("user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet("", Name = "users")]
        [ResponseCache(CacheProfileName = "EntityCache")]
        public IActionResult Index([FromQuery] PaginationQueryParameters parameters)
        {
            var users = _userService.GetByPage<UserDto>(parameters);

            if (users == null || !users.Any())
                return NoContent();

            string jsonString = JsonSerializer.Serialize(new PaginationResponseParams
            {
                currentPage = users.MetaData.CurrentPage,
                pageSize = users.MetaData.PageSize,
                totalSize = users.MetaData.TotalSize,
                totalPages = users.MetaData.TotalPages,
                haveNext = users.MetaData.HaveNext,
                havePrev = users.MetaData.HavePrev,
            });

            HttpContext.Response.Headers.Add("X-Pagination-Params", jsonString);

            return Ok(users);
        }
        [HttpGet("create", Name = "create-user-view")]
        public IActionResult CreateView() => View();
        [HttpPost("create", Name = "create-user")]
        public async Task<IActionResult> Create([FromForm] UserRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return View("CreateView", dto);

            await _authService.RegisterAsync(dto, ["User"]);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
        [HttpPost("delete", Name = "delete-user")]
        public async Task<IActionResult> Delete([FromBody] UserDeleteDto dto)
        {
            await _userService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpGet("update", Name = "update-user-view")]
        public async Task<IActionResult> UpdateView([FromQuery] Guid id)
        {
            var user = await _userService.GetByIdAsync<UserUpdateDto>(id);
            return View(user);
        }
        [HttpPost("update", Name = "update-user")]
        public async Task<IActionResult> Update([FromForm] UserUpdateDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(dto.Id.ToString()))
                return View("UpdateView", dto);

            await _userService.UpdateAsync<UserUpdateDto, UserDto>(dto);
            return RedirectToAction("Index", new { page = 1, pageSize = 10 });
        }
    }
}
