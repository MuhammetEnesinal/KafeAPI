using KafeAPI.Application.Dtos.AuthDtos;
using KafeAPI.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafeAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> Login(TokenDto dto)
        {
            var result = await _authService.GenerateToken(dto);
            return CreateResponse(result);
        }
    }
}
