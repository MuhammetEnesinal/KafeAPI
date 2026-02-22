using FluentValidation;
using KafeAPI.Application.Dtos.TableDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.Table
{
    public class AddTableValidator : AbstractValidator<CreateTableDto>
    {
        public AddTableValidator() {
            RuleFor(x => x.TableNumber)
            .NotEmpty()
            .WithMessage("Masa Numarası boş olamaz")
            .GreaterThan(0)
            .WithMessage("Masa numarası 0'dan büyük olmalıdır.");

            RuleFor(x => x.Capacity)
             .NotEmpty()
             .WithMessage("Masa Kapasitesi boş olamaz")
             .GreaterThan(0)
             .WithMessage("Masa Kapasitesi 0'dan büyük olmalıdır.");
        }            




                
    }
}