using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface IUserServices
    {
        Task<ResponseDto<object>> Register(RegisterDto dto);
        Task<ResponseDto<object>> RegisterDefault(RegisterDto dto); 
        Task<ResponseDto<object>> CreateRole(string roleName);
        Task<ResponseDto<object>> AddToRole(string email, string roleName);

    }
}
