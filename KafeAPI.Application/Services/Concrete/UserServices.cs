using FluentValidation;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Dtos.UserDto;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Concrete
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterDto> _registerValidator;
        public UserServices(IUserRepository userRepository, IValidator<RegisterDto> registerValidator)
        {
            _userRepository = userRepository;
            _registerValidator = registerValidator;
        }

        public async Task<ResponseDto<object>> AddToRole(string email, string roleName)
        {
            try
            {
               var result=await _userRepository.AddRoleToUserAsync(email, roleName);
                if(result)
                {
                    return new ResponseDto<object> { Success = true, Data = null, Message = "Kullanıcı role eklendi."};
                }
                return new ResponseDto<object> { Success = false, Data = null, Message = "Kullanıcı role eklenemedi.", ErrorCode = ErrorCodes.BadRequest };


            }
            catch (Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata oluştu.", ErrorCode = ErrorCodes.Exception };
            }
        }

        public async Task<ResponseDto<object>> CreateRole(string roleName)
        {
            try
            {
                var result =await _userRepository.CreateRoleAsync(roleName);
                
                if(result)
                {
                  return new ResponseDto<object> { Success = true, Data = null, Message = "Rol oluşturuldu."};

                }
                
                  return new ResponseDto<object> { Success = false, Data = null, Message = "Rol oluşturulamadı.", ErrorCode = ErrorCodes.BadRequest };
                
            
            }
            catch (Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata oluştu.", ErrorCode = ErrorCodes.Exception };
            }
        }

        public async Task<ResponseDto<object>> Register(RegisterDto dto)
        {
            try
            {
                var validate=await _registerValidator.ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = validate.Errors.FirstOrDefault().ErrorMessage, ErrorCode = ErrorCodes.ValidationError };

                }
                var result =await _userRepository.RegisterAsync(dto);
                if (result.Succeeded)
                {
                    return new ResponseDto<object> { Success = true, Data = null, Message = "Kayıt Başarılı.", ErrorCode = null };

                }
                else
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = result.Errors.FirstOrDefault().Description };

                }
            }
            catch(Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata oluştu.", ErrorCode = ErrorCodes.Exception };
            }


        }

        public async Task<ResponseDto<object>> RegisterDefault(RegisterDto dto)
        {
            try
            {
                var validate = await _registerValidator.ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = validate.Errors.FirstOrDefault().ErrorMessage, ErrorCode = ErrorCodes.ValidationError };

                }
                var result = await _userRepository.RegisterAsync(dto);
                if (result.Succeeded)
                {
                    var roleResult =await _userRepository.AddRoleToUserAsync(dto.Email, "user");
                    if (roleResult)
                    {
                        return new ResponseDto<object> { Success = true, Data = null, Message = "Kayıt Başarılı.", ErrorCode = null };

                    }
                    else
                    {
                        return new ResponseDto<object> { Success = false, Data = null, Message = "Kullanıcı oluşturuldu rol ataması yaparken hata oluştu", ErrorCode = ErrorCodes.BadRequest };

                    }

                }
                else
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = result.Errors.FirstOrDefault().Description };

                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata oluştu.", ErrorCode = ErrorCodes.Exception };
            }

        }
    }
}
