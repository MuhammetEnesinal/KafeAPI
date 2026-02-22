using AutoMapper;
using FluentValidation;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Dtos.TableDtos;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Concrete
{
    public class TableServices:ITableServices
    {
        private readonly IGenericRepository<Table> _tableRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTableDto> _createTableValidator;
        public TableServices(IGenericRepository<Table> tableRepository, IMapper mapper, IValidator<CreateTableDto> createTableValidator)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
            _createTableValidator = createTableValidator;
        }
        public async Task<ResponseDto<object>> AddTable(CreateTableDto dto)
        {
            try
            {
                var validate = await _createTableValidator.ValidateAsync(dto);
                if (!validate.IsValid) { 
                    
                    return new ResponseDto<object> { Success = false,Data=null,Message=string.Join(",",validate.Errors.Select(x=>x.ErrorMessage)),ErrorCodes=ErrorCodes.ValidationError};
                }
                var checkTable = await _tableRepository.GetByIdAsync(dto.TableNumber);
                if (checkTable != null) {

                    return new ResponseDto<object> { Success = false, Data = null, Message = "Eklemek istediginiz masa numarası mevcuttur.", ErrorCodes = ErrorCodes.DuplicateError };

                }
                var result=_mapper.Map<Table>(dto);
                await _tableRepository.AddAsync(result);
                return new ResponseDto<object> { Success = true, Data = null ,Message="Masa basarili bir sekilde olusturuldu."};
            }
            catch (Exception ex) { 
            return new ResponseDto<object> { Success=false,Data=null,Message="Bir Hata oluştu",ErrorCodes=ErrorCodes.Exception}; 
                
             }
        }

        public Task<ResponseDto<object>> DeleteTable(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<List<ResultTableDto>>> GetAllTables()
        {
            try
            {
                var rp =await _tableRepository.GetAllAsync();
                if(rp.Count() == 0)
                {
                    return new ResponseDto<List<ResultTableDto>> { Success = false, Data = null, Message = "Masalar Bulunamadi.", ErrorCodes = ErrorCodes.NotFound };
                }
                var result=_mapper.Map<List<ResultTableDto>>(rp);
                return new ResponseDto<List<ResultTableDto>> { Success = true, Data = result };
            }
            catch (Exception ex) {

                return new ResponseDto<List<ResultTableDto>>() { Success = false, Data = null, Message = "Bir sorun oluştu.", ErrorCodes =ErrorCodes.Exception };
            }
        }

        public async Task<ResponseDto<DetailTableDto>> GetByIdTable(int id)
        {
            try
            {
                var rp=await _tableRepository.GetByIdAsync(id);
                if (rp == null)
                {
                    return new ResponseDto<DetailTableDto> { Success = false, Data = null, Message = "Masa bulunamadı", ErrorCodes = ErrorCodes.NotFound };

                }
                var result =_mapper.Map<DetailTableDto>(rp);
                return new ResponseDto<DetailTableDto> {Success = true, Data = result };

            }
            catch (Exception ex) {

                return new ResponseDto<DetailTableDto>{ Success = false, Data = null, Message = "Bir hata olustu.", ErrorCodes = ErrorCodes.Exception };
                
            }
        }

        public Task<ResponseDto<DetailTableDto>> GetByTableNumber(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<object>> UpdateTable(UpdateTableDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
