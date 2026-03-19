using FluentValidation;
using KafeAPI.Application.Dtos.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.OrderItem
{
    public class UpdateOrderItemValidator: AbstractValidator<UpdateOrderItemDto>
    {

        public UpdateOrderItemValidator() {

            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Miktar alanı boş bırakılamaz.").GreaterThan(0).WithMessage("Siparis adeti bos olamaz");

        }
    }
}
