using KafeAPI.Application.Dtos.MenuItemDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface IMenuItemServices
    {
        Task<ResponseDto<List<ResultMenuItemDto>>> GetAllMenuItems();
        Task<ResponseDto< DetailMenuItemDto>> GetByIdMenuItems(int id);
        Task <ResponseDto<object>> AddMenuItem(CreateMenuItemDto dto);
        Task <ResponseDto<object>> UpdateMenuItem(UpdateMenuItemDto dto);
        Task <ResponseDto<object>> DeleteMenuItem(int id);
    }
}
