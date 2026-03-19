using KafeAPI.Application.Interfaces;
using KafeAPI.Domain.Entities;
using KafeAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Persistence.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrderWithDetailAsync()
        {
            var result =await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.MenuItem)
                .ThenInclude(m => m.Category)
                .ToListAsync();

            return result;
        }
    

     public async Task<Order> GetOrderByIdWithDetailAsync(int orderId)
        {
            var result = await _context.Orders
                .Where(x => x.Id == orderId)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.MenuItem)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
