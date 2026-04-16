using KafeAPI.Application.Dtos.CafeInfoDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface ICafeInfoServices
    {
        Task<ResponseDto<List<ResultCafeInfoDto>>> GetAllCafeInfos();
        Task<ResponseDto<DetailCafeInfoDto>> GetByIdCafeInfo(int id);
        Task<ResponseDto<object>> AddCafeInfo(CreateCafeInfoDto dto);
        Task<ResponseDto<object>> UpdateCafeInfo(UpdateCafeInfoDto dto);
        Task<ResponseDto<object>> DeleteCafeInfo(int id);
    }
}
