using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Interfaces
{
    public interface ITableRepository
    {
        Task<Table> GetByTableNumberAsync(int tableNumber);


    }
}
