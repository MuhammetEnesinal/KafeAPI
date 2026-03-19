using KafeAPI.Application.Dtos.OrderItemDtos;
using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Dtos.OrderDtos
{
    public class ResultOrderDto
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Status { get; set; }
        public List<ResultOrderItemDto> OrderItems { get; set; }
    }
}
