using KafeAPI.Application.Dtos.UserDto;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Application.Services.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafeAPI.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userServices.Register(dto);
            return CreateResponse(result);

        }
        [Authorize(Roles = "admin,employe")]
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _userServices.CreateRole(roleName);
            return CreateResponse(result);
        }
        [Authorize(Roles = "admin,employe")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleToUser(string email, string roleName)
        {
            var result = await _userServices.AddToRole(email, roleName);
            return CreateResponse(result);
        }


        [HttpPost("registerDefault ")]
        public async Task<IActionResult> RegisterDefault(RegisterDto dto)
        {
            var result = await _userServices.RegisterDefault(dto);
            return CreateResponse(result);
        }
    }
}

