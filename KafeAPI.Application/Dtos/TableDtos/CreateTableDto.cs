using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Dtos.TableDtos
{
    public class CreateTableDto
    {
        public int Id { get; set; }

        public int TableNumber { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }
    }
}
