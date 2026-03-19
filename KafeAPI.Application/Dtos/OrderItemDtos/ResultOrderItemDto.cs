using KafeAPI.Application.Dtos.MenuItemDtos;
using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Dtos.OrderItemDtos
{
    public class ResultOrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public DetailMenuItemDto MenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
