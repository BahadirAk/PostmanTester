using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostmanTester.Application.Interfaces.Services;

namespace PostmanTester.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _userService.GetByTokenAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}
