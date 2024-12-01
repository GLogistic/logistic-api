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
        [HttpPost("create", Name = "create-user")]
        public async Task<IActionResult> Create([FromForm] UserRegistrationDto dto)
        {
            await _authService.RegisterAsync(dto, ["User"]);
            return Ok();
        }
        [HttpPost("delete", Name = "delete-user")]
        public async Task<IActionResult> Delete([FromBody] UserDeleteDto dto)
        {
            await _userService.DeleteByIdAsync(dto.Id);
            return Ok();
        }
        [HttpPut("update", Name = "update-user")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var user = await _userService.UpdateAsync<UserUpdateDto, UserDto>(dto);
            return Ok(user);
        }
    }
}
