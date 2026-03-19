using AutoMapper;
using FluentValidation;
using KafeAPI.Application.Dtos.CategoryDtos;
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
    public class CategoryServices : ICategoryServices
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMenuItemRepository _menuItemRepository;


        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryDto> _createCategoryValidator;
        private readonly IValidator<UpdateCategoryDto> _updateCategoryValidator;
        public CategoryServices(IGenericRepository<Category> categoryRepository, IMapper mapper, IValidator<CreateCategoryDto> createCategoryValidator, IValidator<UpdateCategoryDto> updateCategoryValidator, IMenuItemRepository menuItemRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
            _menuItemRepository = menuItemRepository;
        }
        public async Task<ResponseDto<object>> AddCategory(CreateCategoryDto dto)
        {
            try
            {
                var validate =await _createCategoryValidator.ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    return new ResponseDto<object> { Success = false, Data = null, Message =string.Join(" , ", validate.Errors.Select(x=>x.ErrorMessage)), ErrorCode = ErrorCodes.ValidationError };
                }
                var category = _mapper.Map<Category>(dto);
                await _categoryRepository.AddAsync(category);
                return new ResponseDto<object> { Success = true ,Data=null,Message="Kategori oluşturuldu"};

            }
            catch (Exception ex)
            {
                return new ResponseDto<object> { Success=false,Data=null,Message="Bir Hata Oluştu",ErrorCode=ErrorCodes.Exception};

            }

        }

        public async Task<ResponseDto<object>> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {

                    return new ResponseDto<object> { Success = false,Data=null, Message = "Kategori Bulununamadı", ErrorCode = ErrorCodes.NotFound };

                }

                await _categoryRepository.DeleteAsync(category);
                return new ResponseDto<object> { Success = true, Data = null, Message = "Kategori Silindi" };
            }
            catch (Exception ex) { 
                return new ResponseDto<object> { Success=false, Message ="Bir hata oluştu",ErrorCode = ErrorCodes.Exception };
            }

          
 
        }

        public async Task<ResponseDto<List<ResultCategoryDto>>> GetAllCategories()
        {
            try
            {

                var categories = await _categoryRepository.GetAllAsync();
                if (categories.Count() == 0)
                {
                    return new ResponseDto<List<ResultCategoryDto>> { Success = false, Message = "Kategori Bulunamadı.", ErrorCode = ErrorCodes.NotFound };
                }
               
                var result = _mapper.Map<List<ResultCategoryDto>>(categories);
                return new ResponseDto<List<ResultCategoryDto>> { Success = true, Data = result };
            }
            catch (Exception ex) {

                return new ResponseDto<List<ResultCategoryDto>> { Success = false, Message = "Bir hata oluştu", ErrorCode = ErrorCodes.Exception };
            }


        }

        public async Task<ResponseDto<DetailCategoryDto>> GetByIdCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return new ResponseDto<DetailCategoryDto> { Success = false, Message = "Kategori Bulunamadı", ErrorCode = ErrorCodes.NotFound };
                }
                var menuItems = await _menuItemRepository.GetMenuItemFilterByCategoryId(id);
                var result = _mapper.Map<DetailCategoryDto>(category);
                var newlist = _mapper.Map<List<CategoriesMenuItemDto>>(menuItems);
                return new ResponseDto<DetailCategoryDto>{ Success = true, Data = result };
            }
            catch (Exception ex) { 
            return new ResponseDto<DetailCategoryDto> { Success = false, Message = "Bir hata oluştu", ErrorCode =ErrorCodes.Exception };
            }


     
        }

        public async Task<ResponseDto<List<ResultCategoriesWithMenuDto>>> GetCategoriesWithMenuItem()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                if (categories.Count() == 0)
                {
                    return new ResponseDto<List<ResultCategoriesWithMenuDto>> { Success = false, Message = "Kategori Bulunamadı.", ErrorCode = ErrorCodes.NotFound };
                }

                var result = _mapper.Map<List<ResultCategoriesWithMenuDto>>(categories);
               
                foreach (var item in result)
                {
                    var listMenuItems = await _menuItemRepository.GetMenuItemFilterByCategoryId(item.Id);
                    var newlist = _mapper.Map<List<CategoriesMenuItemDto>>(listMenuItems);
                    item.MenuItems = newlist;
                }

                return new ResponseDto<List<ResultCategoriesWithMenuDto>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<ResultCategoriesWithMenuDto>> { Success = false, Message = "Bir hata oluştu", ErrorCode = ErrorCodes.Exception };
            }
        }

        public async Task<ResponseDto<object>> UpdateCategory(UpdateCategoryDto dto)
        {
            try
            {   
                var validate =await _updateCategoryValidator.ValidateAsync(dto);

                 if (!validate.IsValid) {
                    return new ResponseDto<object> { Success = false, Data = null, Message = string.Join(" , ", validate.Errors.Select(x => x.ErrorMessage)), ErrorCode = ErrorCodes.ValidationError };
                
                
                }

                var categorydb=await _categoryRepository.GetByIdAsync(dto.Id);
                if (categorydb == null)
                {
                    return new ResponseDto<object> { Success = false, Message = "Kategori Bulunamadi.", ErrorCode = ErrorCodes.NotFound };
                }


                var category = _mapper.Map(dto,categorydb);
              
                await _categoryRepository.UpdateAsync(category);
                return new ResponseDto<object> { Success = true, Data = null, Message = "Kategori Güncellendi" };


            }
            catch(Exception ex)
            {
                return new ResponseDto<object> { Success = false, Data = null, Message = "Bir Hata oluştu", ErrorCode = ErrorCodes.Exception };

            }

           
        }
    }
}
