using KafeAPI.Application.Dtos.CategoryDtos;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafeAPI.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryServices _categoryServices;
        private readonly Serilog.ILogger _log;

        public CategoriesController(ICategoryServices categoryServices, Serilog.ILogger log)
        {
            _categoryServices = categoryServices;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            _log.Information("get-categories");
            var result = await _categoryServices.GetAllCategories();
            _log.Information("iget-categories:"+result.Success);
            _log.Warning("wget-categories:" + result.Success);
            _log.Error("eget-categories:" + result.Success);
            _log.Debug("eget-categories:" + result.Success);

            return CreateResponse(result);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCategories(int id)
        {

            var result = await _categoryServices.GetByIdCategory(id);
            return CreateResponse(result);



        }
        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDto dto)
        {
            var result = await _categoryServices.AddCategory(dto);
            return CreateResponse(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto dto)
        {
            var result = await _categoryServices.UpdateCategory(dto);

            return CreateResponse(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryServices.DeleteCategory(id);

            return CreateResponse(result);
        }

        [HttpGet("withmewnuıtems")]
        public async Task<IActionResult> GetAllCategoriesWithMenuItems()
        {
            var result = await _categoryServices.GetCategoriesWithMenuItem();
            return CreateResponse(result);


        }
    }
}
