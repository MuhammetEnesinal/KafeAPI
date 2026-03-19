using KafeAPI.Application.Dtos.OrderItemDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface IOrderItemServices
    {

        Task<ResponseDto<List<ResultOrderItemDto>>> GetAllOrderItems();
 
        Task<ResponseDto<DetailOrderItemDto>> GetOrderItemById(int id);
        Task<ResponseDto<object>> AddOrderItem(CreateOrderItemDto dto);
        Task<ResponseDto<object>> UpdateOrderItem(UpdateOrderItemDto dto);
        Task<ResponseDto<object>> DeleteOrderItem(int id);
    }
}
