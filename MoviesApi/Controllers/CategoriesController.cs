using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.BLL.Interfaces;
using Movies.DAL.Data;
using Movies.DAL.Entities;
using Movies.PL.DTOs;

namespace Movies.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Get All Categories
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetAllCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            return Ok(categories);
        }
        //Get By Id
        [HttpGet("{id}")]
        public async  Task<ActionResult<Category>>GetById(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if(category is null)return NotFound($"Category with id {id} is not found ");
            return Ok(category);
        }
        //Create Category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryDto categoryDto)
        {
            var Createdcategory = new Category
            {
                Name = categoryDto.Name
            };
            await _unitOfWork.CategoryRepository.AddAsync(Createdcategory);
            await _unitOfWork.Complete();
            return Ok(Createdcategory);
        }

        //Edit Category 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound($"Category not found  with {id}");
            category.Name = dto.Name;
            await _unitOfWork.Complete();
            return Ok(category);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound($"Category not found  with {id}");
            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.Complete();
            return Ok(category);
        }


    }
}
