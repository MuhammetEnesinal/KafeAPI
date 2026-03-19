using AutoMapper;
using FluentValidation;
using KafeAPI.Application.Dtos.OrderDtos;
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
    public class OrderServices : IOrderServices
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<MenuItem> _menuItemRepository;
        private readonly IGenericRepository<Table> _tableRepository;
        private readonly IOrderRepository _orderRepository2;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createOrderValidator;
        private readonly IValidator<UpdateOrderDto> _updateOrderValidator;
        public OrderServices(IGenericRepository<Order> orderRepository, IMapper mapper, IValidator<CreateOrderDto> createOrderValidator, IValidator<UpdateOrderDto> updateOrderValidator, IGenericRepository<OrderItem> orderItemRepository, IOrderRepository orderRepository2, IGenericRepository<MenuItem> menuItemRepository, IGenericRepository<Table> tableRepository = null)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _createOrderValidator = createOrderValidator;
            _updateOrderValidator = updateOrderValidator;
            _orderItemRepository = orderItemRepository;
            _orderRepository2 = orderRepository2;
            _menuItemRepository = menuItemRepository;
            _tableRepository = tableRepository;
        }

        public async Task<ResponseDto<object>> AddOrder(CreateOrderDto dto)
        {
            try
            {
                var validate = await _createOrderValidator.ValidateAsync(dto);

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
                var orderEntity = _mapper.Map<Order>(dto);
                orderEntity.Status = OrderStatus.Hazirlaniyor;
                orderEntity.CreatedAt = DateTime.Now;
                decimal totalPrice = 0;
                foreach (var item in orderEntity.OrderItems)
                {
                    item.MenuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId);
                    item.Price=item.MenuItem.Price * item.Quantity;
                    totalPrice += item.Price;
                }
                orderEntity.TotalPrice = totalPrice;

                await _orderRepository.AddAsync(orderEntity);

                var table = await _tableRepository.GetByIdAsync(dto.TableId);
                
                table.IsActive = false;
                await _tableRepository.UpdateAsync(table);


                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis basariyla eklendi.",
                    Data = null
                };


            }
            catch (Exception ex)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        

        public async Task<ResponseDto<object>> DeleteOrder(int orderId)
        {
            try
            {
                var orderdb = await _orderRepository.GetByIdAsync(orderId);
                if (orderdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                await _orderRepository.DeleteAsync(orderdb);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis basariyla silindi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<List<ResultOrderDto>>> GetAllOrder()
        {
            try
            {
                var orderdb = await _orderRepository.GetAllAsync();
                var orderItems = await _orderItemRepository.GetAllAsync();

                if (orderdb.Count() == 0)
                {
                    return new ResponseDto<List<ResultOrderDto>>
                    {
                        Success = false,
                        Message = "Herhangi bir siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };

                }
                var result = _mapper.Map<List<ResultOrderDto>>(orderdb);
                return new ResponseDto<List<ResultOrderDto>>
                {
                    Success = true,
                    Data = result,

                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<List<ResultOrderDto>>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<List<ResultOrderDto>>> GetAllOrdersWithDetail()
        {

            try
            {
                var orderdb = await _orderRepository2.GetAllOrderWithDetailAsync(); 


                if (orderdb.Count() == 0)
                {
                    return new ResponseDto<List<ResultOrderDto>>
                    {
                        Success = false,
                        Message = "Herhangi bir siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };

                }
                var result = _mapper.Map<List<ResultOrderDto>>(orderdb);
                return new ResponseDto<List<ResultOrderDto>>
                {
                    Success = true,
                    Data = result,

                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<List<ResultOrderDto>>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<DetailOrderDto>> GetOrderById(int orderId)
        {
            try
            {
                var oderdb = await _orderRepository2.GetOrderByIdWithDetailAsync(orderId);
                if (oderdb == null)
                {
                    return new ResponseDto<DetailOrderDto>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                var result = _mapper.Map<DetailOrderDto>(oderdb);
                return new ResponseDto<DetailOrderDto>
                {
                    Success = true,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<DetailOrderDto>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
        public async Task<ResponseDto<object>> UpdateOrder(UpdateOrderDto dto)
        {
            try
            {
                var validate = await _updateOrderValidator.ValidateAsync(dto);
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
                var orderdb = await _orderRepository.GetByIdAsync(dto.Id);
                if (orderdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                var result = _mapper.Map(dto, orderdb);
                
                result.UpdateAt = DateTime.Now;

                decimal totalPrice = 0;

                foreach (var item in result.OrderItems)
                {
                    item.MenuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId);
                    item.Price = item.MenuItem.Price * item.Quantity;
                    totalPrice += item.Price;
                }
                result.TotalPrice = totalPrice;
                await _orderRepository.UpdateAsync(result);

                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis basariyla guncellendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<object>> UpdateOrderStatusHazir(int orderId)
        {
            try
            {
               
                var orderdb = await _orderRepository.GetByIdAsync(orderId);
                if (orderdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

               orderdb.Status = OrderStatus.Hazir;
                
                await _orderRepository.UpdateAsync(orderdb);

                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis durumu hazir olarak guncellendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<object>> UpdateOrderStatusTeslimEdildi(int orderId)
        {
            try
            {

                var orderdb = await _orderRepository.GetByIdAsync(orderId);
                if (orderdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                orderdb.Status = OrderStatus.TeslimEdildi;

                await _orderRepository.UpdateAsync(orderdb);

                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis durumu teslim edildi olarak guncellendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<object>> UpdateOrderStatusİptalEdildi(int orderId)
        {
            try
            {

                var orderdb = await _orderRepository.GetByIdAsync(orderId);
                if (orderdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                orderdb.Status = OrderStatus.IptalEdildi;

                await _orderRepository.UpdateAsync(orderdb);

                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis durumu iptal edildi olarak guncellendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<object>> UpdateOrderStatusOdendi(int orderId)
        {
            try
            {

                var orderdb = await _orderRepository.GetByIdAsync(orderId);
                if (orderdb == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Siparis bulunamadi.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                orderdb.Status = OrderStatus.Odendi;
                var table = await _tableRepository.GetByIdAsync(orderdb.TableId);
                table.IsActive = true;
                await _tableRepository.UpdateAsync(table);

                await _orderRepository.UpdateAsync(orderdb);

                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Siparis durumu odendi olarak guncellendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
    }
}