using FluentValidation;
using KafeAPI.Application.Dtos.MenuItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.MenuItem
{
    public class AddMenuItemValidator :AbstractValidator<CreateMenuItemDto>
    {
        public AddMenuItemValidator() {
         
            RuleFor(x => x.Name).NotEmpty().WithMessage("Menu Item Adı Boş olamaz").Length(2,40).WithMessage("Menu Item Adı 2 ile 40 karakter arasında olmak zorundadır");


            RuleFor(x => x.Description).NotEmpty().WithMessage("Menu Item Açıklaması boş olamaz").Length(5, 100).WithMessage("Menu Item Açıklaması 5 ile 100 karakter arasında olmak  zorundadır ");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Menu Item fiyatı boş olamaz").GreaterThan(0).WithMessage("Menu Item Fiyatı 0 dan büyük olmak zorundadır");

            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Menu Item fotograf url boş olamaz");



        }
    }
}
