using KafeAPI.Application.Dtos.AuthDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Helpers;
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

        public AuthServices(TokenHelpers tokenHelpers)
        {
            _tokenHelpers = tokenHelpers;
        }

        public async Task<ResponseDto<object>> GenerateToken(TokenDto dto)
        {
            try
            {
               var checkuser=dto.Email=="admin@admin.com" ? true : false;
                if (checkuser)
                {
                    string token = _tokenHelpers.GenerateToken(dto);
                    return new ResponseDto<object>
                    {
                        Success = true,
                        Data = token

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
