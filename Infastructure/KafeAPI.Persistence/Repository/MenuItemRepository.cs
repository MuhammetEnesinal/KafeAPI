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
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;

        public MenuItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetMenuItemFilterByCategoryId(int categoryId)
        {
            var result =await _context.MenuItems.Where(m => m.CategoryId == categoryId).ToListAsync();

            return result; 
        }

       
    }
}
