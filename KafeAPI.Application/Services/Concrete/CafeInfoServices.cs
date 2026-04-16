using AutoMapper;
using FluentValidation;
using KafeAPI.Application.Dtos.CafeInfoDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Concrete
{
    public class CafeInfoServices : ICafeInfoServices
    {
        private readonly IGenericRepository<CafeInfo> _cafeInfoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCafeInfoDto> _createCafeInfoValidator;
        private readonly IValidator<UpdateCafeInfoDto> _updateCafeInfoValidator;

        public CafeInfoServices(IGenericRepository<CafeInfo> cafeInfoRepository, IMapper mapper, IValidator<CreateCafeInfoDto> createCafeInfoValidator, IValidator<UpdateCafeInfoDto> updateCafeInfoValidator)
        {
            _cafeInfoRepository = cafeInfoRepository;
            _mapper = mapper;
            _createCafeInfoValidator = createCafeInfoValidator;
            _updateCafeInfoValidator = updateCafeInfoValidator;
        }

        public async Task<ResponseDto<object>> AddCafeInfo(CreateCafeInfoDto dto)
        {
            try
            {
                var validationResult = await _createCafeInfoValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)),
                        Data = null,
                        ErrorCode = ErrorCodes.ValidationError
                    };
                }
                var cafeInfo = _mapper.Map<CafeInfo>(dto);
                await _cafeInfoRepository.AddAsync(cafeInfo);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Kafe bilgisi başarıyla eklendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<object>> DeleteCafeInfo(int id)
        {
            try
            {
                var existingCafeInfo = await _cafeInfoRepository.GetByIdAsync(id);
                if (existingCafeInfo == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Kafe bilgisi bulunamadı.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                await _cafeInfoRepository.DeleteAsync(existingCafeInfo);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Kafe bilgisi başarıyla silindi.",
                    Data = null
                };
            }
            catch (Exception ex)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<List<ResultCafeInfoDto>>> GetAllCafeInfos()
        {
            try
            {
                var cafeInfo=await _cafeInfoRepository.GetAllAsync();
                if(cafeInfo ==null || !cafeInfo.Any())
                {
                    return new ResponseDto<List<ResultCafeInfoDto>>
                    {
                        Success = false,
                        Message = "Kafe bilgisi bulunamadı.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                var result = _mapper.Map<List<ResultCafeInfoDto>>(cafeInfo);
                return new ResponseDto<List<ResultCafeInfoDto>>
                {
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
              return new ResponseDto<List<ResultCafeInfoDto>>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public  async Task<ResponseDto<DetailCafeInfoDto>> GetByIdCafeInfo(int id)
        {
            try
            {

                var cafeInfo =await _cafeInfoRepository.GetByIdAsync(id);
                if (cafeInfo == null)
                {
                    return new ResponseDto<DetailCafeInfoDto>
                    {
                        Success = false,
                        Message = "Kafe bilgisi bulunamadı.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                var result = _mapper.Map<DetailCafeInfoDto>(cafeInfo);
                return new ResponseDto<DetailCafeInfoDto>
                {
                    Success = true,
                    Data = result
                }; 
            }
            catch (Exception ex)
            {

                return new ResponseDto<DetailCafeInfoDto>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
            }

        public async Task<ResponseDto<object>> UpdateCafeInfo(UpdateCafeInfoDto dto)
        {
            try
            {
                var validationResult = await _updateCafeInfoValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)),
                        Data = null,
                        ErrorCode = ErrorCodes.ValidationError
                    };
                }
                var existingCafeInfo = await _cafeInfoRepository.GetByIdAsync(dto.Id);
                if (existingCafeInfo == null)
                {
                    return new ResponseDto<object>
                    {
                        Success = false,
                        Message = "Kafe bilgisi bulunamadı.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }

                _mapper.Map(dto, existingCafeInfo);
                await _cafeInfoRepository.UpdateAsync(existingCafeInfo);
                return new ResponseDto<object>
                {
                    Success = true,
                    Message = "Kafe bilgisi başarıyla güncellendi.",
                    Data = null
                };

            }
            catch (Exception ex)
            {

                return new ResponseDto<object>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
    }
}
