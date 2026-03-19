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
    public class TableRepository: ITableRepository
    {
        private readonly AppDbContext _context;

        public TableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Table>> GetAllActiveTablesAsync()
        {
            var result = await _context.Tables.Where(t => t.IsActive).ToListAsync();
            return result;
        }

        public async Task<Table> GetByTableNumberAsync(int tableNumber)
        {
            var result = await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
            return result;
        }
    }
}
