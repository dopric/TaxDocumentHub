using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxDocumentHub.Api.Data;
using TaxDocumentHub.Api.Domain;
using TaxDocumentHub.Api.DTOs;
using TaxDocumentHub.Api.Extensions;

namespace TaxDocumentHub.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var userId = User.GetUserId();
            var categories = await _dbContext.Categories.
                Where(c => c.UserId == userId)
                .Select(c => new CategoryResponseDto(c.Id, c.Name))
               .ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto request)
        {
            var userId = User.GetUserId();
            var category = new DocumentCategory
            {
                Name = request.Name,
                UserId = userId
            };
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, new CategoryResponseDto(category.Id, category.Name));
        }
    }
}
