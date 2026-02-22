using AutoMapper;
using FluentValidation;
using KafeAPI.Application.Dtos.MenuItemDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
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
    public class MenuItemServices : IMenuItemServices
    {
        private readonly IGenericRepository<MenuItem> _menuItemRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMenuItemDto> _createMenuItemValidator;
        private readonly IValidator<UpdateMenuItemDto> _updateMenuItemValidator;
        public MenuItemServices(IGenericRepository<MenuItem> menuItemRepository, IGenericRepository<Category> categoryRepository, IMapper mapper, IValidator<CreateMenuItemDto> createMenuItemValidator, IValidator<UpdateMenuItemDto> updateMenuItemValidator)
        {

            _menuItemRepository=menuItemRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createMenuItemValidator=createMenuItemValidator;
            _updateMenuItemValidator=updateMenuItemValidator;
        }
        public async Task<ResponseDto<object>> AddMenuItem(CreateMenuItemDto dto)
        {
            try
            {
                var validate = await _createMenuItemValidator.ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = string.Join(",", validate.Errors.Select(x => x.ErrorMessage)), ErrorCodes = ErrorCodes.ValidationError };
                }
                var checkcategory = await _categoryRepository.GetByIdAsync(dto.CategoryId);
                if (checkcategory ==null)
                {
                    return new ResponseDto<object> { Success = false, Data = dto, Message = "Eklemek istediğiniz kategori bulunamadı.", ErrorCodes = ErrorCodes.NotFound };

                }
                var menyItem = _mapper.Map<MenuItem>(dto);
                await _menuItemRepository.AddAsync(menyItem);
                return new ResponseDto<object> { Success = true, Data = null, Message = "Menu Item başarılı bir şekilde eklendi." };
            }
            catch (Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata Oluştu",ErrorCodes =ErrorCodes.Exception };

            }
        
        }

        public async Task<ResponseDto<object>> DeleteMenuItem(int id)
        {
            try {

                var menuItem = await _menuItemRepository.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = "Menu Item bulunamadı.", ErrorCodes = ErrorCodes.NotFound };
                }
                
                await _menuItemRepository.DeleteAsync(menuItem);
                return new ResponseDto<object> { Success = true, Data = null, Message = "Menu Item başarılı bir şekilde silindi." };
            }
            catch(Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata oluştu.", ErrorCodes = ErrorCodes.Exception };
            }

        }

        public async Task<ResponseDto<List<ResultMenuItemDto>>> GetAllMenuItems()
        {
            try
            {
                var menuItems = await _menuItemRepository.GetAllAsync();
                var category = await _categoryRepository.GetAllAsync();
                if (menuItems.Count() == 0)
                {
                    return new ResponseDto<List<ResultMenuItemDto>> { Success = false, Data = null, Message = "Menu Items bulunmadı", ErrorCodes = ErrorCodes.NotFound };
                }
                var result = _mapper.Map<List<ResultMenuItemDto>>(menuItems);
                return new ResponseDto<List<ResultMenuItemDto>> { Success = true, Data = result };

            }
            catch (Exception ex)
            {
                {
                    return new ResponseDto<List<ResultMenuItemDto>> { Success = false, Data = null, Message = "Bir Hata oluştu", ErrorCodes = ErrorCodes.Exception };

                }


            }
        }

        public async Task<ResponseDto<DetailMenuItemDto>> GetByIdMenuItems(int id)
        {
            var memuItem=await _menuItemRepository.GetByIdAsync(id);
            var category =await _categoryRepository.GetByIdAsync(memuItem.CategoryId);
            if (memuItem == null)
            {
                return new ResponseDto<DetailMenuItemDto> { Success = false, Data = null, Message = "Menu Item bulunamadı." ,ErrorCodes=ErrorCodes.NotFound};
            }
            var result=_mapper.Map<DetailMenuItemDto>(memuItem);
            return new ResponseDto<DetailMenuItemDto> {Success=true,Data=result };
        }

        public async Task<ResponseDto<object>> UpdateMenuItem(UpdateMenuItemDto dto)
        {
            try
            {   
                var validate =await _updateMenuItemValidator.ValidateAsync(dto);
                if(!validate.IsValid)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = string.Join(",", validate.Errors.Select(x => x.ErrorMessage)) ,ErrorCodes=ErrorCodes.ValidationError};
                }
                var menuItem = await _menuItemRepository.GetByIdAsync(dto.Id);

                if (menuItem == null)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message = "Menu Item Bulunamadı", ErrorCodes = ErrorCodes.NotFound };
                }
                var checkcategory = await _categoryRepository.GetByIdAsync(dto.CategoryId);
                if (checkcategory == null)
                {
                    return new ResponseDto<object> { Success = false, Data = dto, Message = "Eklemek istediğiniz kategori bulunamadı.", ErrorCodes = ErrorCodes.NotFound };

                }

                var newmenuItem = _mapper.Map(dto, menuItem);
                await _menuItemRepository.UpdateAsync(newmenuItem);
                return new ResponseDto<object> { Success = true, Data = null ,Message="Menu Item basariyla güncellendi."};
            }
            catch (Exception ex) {

                return new ResponseDto<object> { Success = false, Message = "Bir hata oluştu.", Data = null, ErrorCodes = ErrorCodes.Exception };
            }

            
        }
    }
}
