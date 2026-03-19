using FluentValidation;
using KafeAPI.Application.Dtos.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.OrderItem
{
    public class AddOrderItemValidator:AbstractValidator<CreateOrderItemDto>
    {

        public AddOrderItemValidator()
        {
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Miktar alanı boş bırakılamaz.").GreaterThan(0).WithMessage("Siparis adeti bos olamaz");
        }
    }
}
