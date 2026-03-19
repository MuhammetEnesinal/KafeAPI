using KafeAPI.Application.Dtos.UserDto;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Application.Services.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafeAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserServices _userServices;


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userServices.Register(dto);
            return CreateResponse(result);

        }

    }
}

