using KafeAPI.Application.Dtos.AuthDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface IAuthServices
    {
        Task<ResponseDto<object>> GenerateToken(TokenDto dto);
    }
}
