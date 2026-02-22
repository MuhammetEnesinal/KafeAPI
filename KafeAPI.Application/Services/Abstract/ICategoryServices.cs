using KafeAPI.Application.Dtos.CategoryDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface ICategoryServices
    {

        Task<ResponseDto<List<ResultCategoryDto>>> GetAllCategories();
        Task<ResponseDto<DetailCategoryDto>> GetByIdCategory(int id);
        Task <ResponseDto<object>> AddCategory(CreateCategoryDto dto);
        Task <ResponseDto<object >>UpdateCategory(UpdateCategoryDto dto);
        Task <ResponseDto<object>>DeleteCategory(int id);

    }
}
