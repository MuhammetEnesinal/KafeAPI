using KafeAPI.Application.Dtos.AuthDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Dtos.UserDto;
using KafeAPI.Application.Helpers;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Concrete
{
    public class AuthServices : IAuthServices
    {
        private readonly TokenHelpers _tokenHelpers;
        private readonly IUserRepository _userRepository;

        public AuthServices(TokenHelpers tokenHelpers, IUserRepository userRepository)
        {
            _tokenHelpers = tokenHelpers;
            _userRepository = userRepository;
        }

        public async Task<ResponseDto<object>> GenerateToken(LoginDto dto)
        {
            try
            {
               var checkuser=await _userRepository.CheckUser(dto.Email);
                if (checkuser.Id !=null)
                {
                    var user = await _userRepository.CheckUserWithPassword(dto);
                    if (user.Succeeded)
                    {
                        var tokendto=new TokenDto
                        {
                            Id = checkuser.Id,
                            Email = checkuser.Email,
                            Role =checkuser.Role
                        };
                        string token = _tokenHelpers.GenerateToken(tokendto);
                        return new ResponseDto<object>
                        {
                            Success = true,
                            Data = new {token=token}

                        };
                    }
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Data = null,
                        Message = "Kullanıcı bulunamadı",
                        ErrorCode = ErrorCodes.Unauthorized

                    };

                }
                return new ResponseDto<object>
                {
                    Success = false,
                    Data = null,
                    Message= "Kullanıcı bulunamadı",
                    ErrorCode = ErrorCodes.Unauthorized

                };


            }   
            catch (Exception ex)
            {
                return new ResponseDto<object>
                {
                    Success = false,
                    Data= null,
                    Message = "Bir Hata olustu",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
    }
}
