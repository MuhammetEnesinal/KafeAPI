using KafeAPI.Application.Dtos.OrderDtos;
using KafeAPI.Application.Dtos.OrderItemDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface IOrderServices
    {
        Task<ResponseDto<List<ResultOrderDto>>> GetAllOrder();
        Task<ResponseDto<DetailOrderDto>> GetOrderById(int orderId);
        Task<ResponseDto<object>> AddOrder(CreateOrderDto dto);
        Task<ResponseDto<object>> UpdateOrder(UpdateOrderDto dto);
        Task<ResponseDto<object>> DeleteOrder(int orderId);

        Task<ResponseDto<List<ResultOrderDto>>> GetAllOrdersWithDetail();
        Task<ResponseDto<object>> UpdateOrderStatusHazir(int orderId);
        Task<ResponseDto<object>> UpdateOrderStatusTeslimEdildi(int orderId);
        Task<ResponseDto<object>> UpdateOrderStatusİptalEdildi(int orderId);
       // Task<ResponseDto<object>> AddOrderItemByOrderId( AddOrderItemByOrderDto dto);
        Task<ResponseDto<object>> UpdateOrderStatusOdendi(int orderId);


    }
}
