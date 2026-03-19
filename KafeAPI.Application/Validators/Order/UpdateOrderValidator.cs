using FluentValidation;
using KafeAPI.Application.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.Order
{
    public class UpdateOrderValidator:AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderValidator()
        {
            //RuleFor(x => x.TotalPrice).NotEmpty().WithMessage("Toplam fiyat boş olamaz.").GreaterThan(0).WithMessage("Siparis ücreti 0 dan büyük olmalıdır.");
        }

    }
}
