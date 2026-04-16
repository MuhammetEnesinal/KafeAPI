using FluentValidation;
using KafeAPI.Application.Dtos.CafeInfoDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.CafeInfo
{
    public class AddCafeInfoValidator : AbstractValidator<CreateCafeInfoDto>
    {
        public AddCafeInfoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kafe Adı boş olamaz.").MaximumLength(100).WithMessage("Kafe Adı 100 karakterden uzun olamaz.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon numarası boş olamaz.").Matches(@"^\d{10}$").WithMessage("Geçerli bir telefon numarası giriniz.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Adres boş olamaz.").MaximumLength(200).WithMessage("Adres 200 karakterden uzun olamaz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email boş olamaz.").EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");



        }
    }
}
