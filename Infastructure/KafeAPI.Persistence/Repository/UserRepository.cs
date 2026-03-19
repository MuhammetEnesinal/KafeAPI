using KafeAPI.Application.Dtos.UserDto;
using KafeAPI.Application.Interfaces;
using KafeAPI.Persistence.Context.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        public UserRepository(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, true, false);
                
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = new AppIdentityUser
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.Phone
            };
            var result =await _userManager.CreateAsync(user, dto.Password);

            return result;

        }
    }
}
