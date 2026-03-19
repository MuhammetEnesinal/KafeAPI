using FluentValidation;
using KafeAPI.Application.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Validators.User
{
    public class RegisterValidator :AbstractValidator<RegisterDto>
    {

        public RegisterValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ad alanı boş bırakılamaz.")
                .MinimumLength(2)
                .WithMessage("Ad alanı en az 2 karakter olmalıdır");

            RuleFor(x => x.Surname)
               .NotEmpty()
               .WithMessage("Surnamealanı boş bırakılamaz.")
               .MinimumLength(2)
               .WithMessage("Surname alanı en az 2 karakter olmalıdır");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress()
                .WithMessage("Geçerli bir email adresi giriniz.");
       
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password alanı boş bırakılamaz.")
                .MinimumLength(6)
                .WithMessage("Password alanı en az 6 karakter olmalıdır.")
                .Matches("[A-Z]")
                .WithMessage("Password alanı en az bir büyük harf içermelidir.")
                .Matches("[a-z]")
                .WithMessage("Password alanı en az bir küçük harf içermelidir.")
                .Matches("[0-9]")
                .WithMessage("Password alanı en az bir rakam içermelidir.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password alanı en az bir özel karakter içermelidir.");
            
            RuleFor(x => x.Phone)
                 .NotEmpty()
                 .WithMessage("Phone alanı boş bırakılamaz.")
                 .Matches(@"^\d{10}$")
                 .WithMessage("Phone alanı 10 haneli bir sayı olmalıdır.");


        }


    }
}
