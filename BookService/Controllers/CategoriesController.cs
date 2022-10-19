using BookService.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private BooksDbContext _dbContext;

        public CategoriesController(BooksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Models.DTO.CategoryResponse>> Get()
        {
            List<Models.DTO.CategoryResponse> Response = new List<Models.DTO.CategoryResponse>();
            var Categories = this._dbContext.Categories.ToList();
            Categories.ForEach(c =>
            {
                Response.Add(new Models.DTO.CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name
                });
            });

            return Ok(Response);
        }
    }
}
