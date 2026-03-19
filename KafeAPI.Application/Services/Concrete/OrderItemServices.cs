using AutoMapper;
using FluentValidation;
using KafeAPI.Application.Dtos.OrderItemDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Concrete
{
    public class OrderItemServices : IOrderItemServices
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderItemDto> _createOrderItemValidator;
        private readonly IValidator<UpdateOrderItemDto> _updateOrderItemValidator;  

        public OrderItemServices(IGenericRepository<OrderItem> orderItemRepository, IMapper mapper, IValidator<CreateOrderItemDto> createOrderItemValidator)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _createOrderItemValidator = createOrderItemValidator;
        }
        public async Task<ResponseDto<object>> AddOrderItem(CreateOrderItemDto dto)
        {
            try
            {
                var validate=await _createOrderItemValidator.ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = string.Join(",",validate.Errors.Select(x=>x.ErrorMessage)),
                        Data = null,
                        ErrorCode = ErrorCodes.ValidationError
                    };

                }

                var orderItem = _mapper.Map<OrderItem>(dto);
                await _orderItemRepository.AddAsync(orderItem);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis itemi basariyla eklendi.",
                    Data = null,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<object>> DeleteOrderItem(int id)
        {
            try
            {
                var checkOrderItem =await _orderItemRepository.GetByIdAsync(id);
                if(checkOrderItem == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis itemi bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                await _orderItemRepository.DeleteAsync(checkOrderItem);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis itemi basariyla silindi.",
                    Data = null,
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<List<ResultOrderItemDto>>> GetAllOrderItems()
        {
            try
            {
                var orderItemdb =await _orderItemRepository.GetAllAsync();
                if(orderItemdb.Count() == 0)
                {
                    return new ResponseDto<List<ResultOrderItemDto>>{
                        Success = false,
                        Message = "Herhangi bir siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                var result= _mapper.Map<List<ResultOrderItemDto>>(orderItemdb);
                return new ResponseDto<List<ResultOrderItemDto>>{
                    Success = true,
                    Message = "Siparisler basariyla getirildi.",
                    Data = result,
                    ErrorCode = null
                };

            }
            catch (Exception ex)
            {
               return new ResponseDto<List<ResultOrderItemDto>>{ 
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
                
            }
        }
        public async Task<ResponseDto<DetailOrderItemDto>> GetOrderItemById(int id)
        {
            try
            {
                var db=await _orderItemRepository.GetByIdAsync(id);
                if (db == null)
                {
                    return new ResponseDto<DetailOrderItemDto>
                    {
                        Success = false,
                        Data = null,
                        Message = "Siparis itemi bulunamadi.",
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                var result = _mapper.Map<DetailOrderItemDto>(db);
                return new ResponseDto<DetailOrderItemDto>
                {
                    Success = true,
                    Data = result,
                   
                };


            }
            catch (Exception ex)
            {
                return new ResponseDto<DetailOrderItemDto>
                {
                    Success = false,
                    Data = null,
                    Message = "Bir Hata olustu.",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
        public async Task<ResponseDto<object>> UpdateOrderItem(UpdateOrderItemDto dto)
        {
            try
            {
                var validate =await _updateOrderItemValidator.ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = string.Join(",", validate.Errors.Select(x => x.ErrorMessage)),
                        Data = null,
                        ErrorCode = ErrorCodes.ValidationError
                    };
                }
                var orderItemdb = await _orderItemRepository.GetByIdAsync(dto.Id);
                if (orderItemdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis itemi bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                var orderItem = _mapper.Map(dto, orderItemdb);
                await _orderItemRepository.UpdateAsync(orderItem);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis itemi basariyla guncellendi.",
                    Data = null,
                };

            } catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
    }
}
