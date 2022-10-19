using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookService.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private BooksDbContext _dbContext;

        public BooksController(BooksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Models.DTO.BookResponse>> Get([FromQuery] Guid? category = null, [FromQuery] string title = null)
        {
            List<Models.DTO.BookResponse> response = new List<Models.DTO.BookResponse>();

            try
            {
                var books = new List<Models.Domain.Book>();

                //TODO: Consider refactor
                if (category != null && !string.IsNullOrEmpty(title))
                    books = this._dbContext.Books.Where(b => b.Category.Id == category && b.Title.ToLower().Contains(title.ToLower())).ToList();
                else if (category != null)
                    books = this._dbContext.Books.Where(b => b.Category.Id == category).ToList();
                else if (!string.IsNullOrEmpty(title))
                    books = this._dbContext.Books.Where(b => b.Title.ToLower().Contains(title.ToLower())).ToList();
                else
                    books = this._dbContext.Books.ToList();

                books.ForEach(b =>
                {
                    response.Add(new Models.DTO.BookResponse
                    {
                        Id = b.Id,
                        Author = b.Author,
                        Title = b.Title,
                        Category = new Models.DTO.CategoryResponse
                        {
                            Id = b.Category.Id,
                            Name = b.Category.Name
                        },
                        Year = b.Year,
                        RateAverage = b.RateAverage
                    });
                });

                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]

        public ActionResult Get(Guid id)
        {
            try
            {
                var book = this._dbContext.Books.Find(id);
                if (book == null)
                    return BadRequest("Book not found");

                return Ok(new Models.DTO.BookResponse
                {
                    Id = book.Id,
                    Author = book.Author,
                    Title = book.Title,
                    Category = new Models.DTO.CategoryResponse
                    {
                        Id = book.Category.Id,
                        Name = book.Category.Name
                    },
                    Year = book.Year,
                    RateAverage = book.RateAverage
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Models.DTO.BookRequest bookRequest)
        {
            try
            {
                var category = this._dbContext.Categories.Find(bookRequest.Category);
                if (category == null)
                    return BadRequest("Category not found");

                Models.Domain.Book book = new Models.Domain.Book()
                {
                    Author = bookRequest.Author,
                    Title = bookRequest.Title,
                    Year = bookRequest.Year,
                    Category = category
                };
                this._dbContext.Books.Add(book);
                this._dbContext.SaveChanges();

                return Ok(new Models.DTO.BookResponse()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Title = book.Title,
                    Category = new Models.DTO.CategoryResponse
                    {
                        Id = book.Category.Id,
                        Name = book.Category.Name
                    },
                    Year = book.Year,
                    RateAverage = book.RateAverage
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut("{id}")]
        public ActionResult Put(Guid id, [FromBody] Models.DTO.BookRequest bookRequest)
        {
            try
            {
                var book = this._dbContext.Books.Find(id);
                if (book == null)
                    return BadRequest("Book not found");

                var category = this._dbContext.Categories.Find(bookRequest.Category);
                if (category == null)
                    return BadRequest("Category not found");

                book.Author = bookRequest.Author;
                book.Title = bookRequest.Title;
                book.Category = category;
                book.Year = bookRequest.Year;
                this._dbContext.Books.Update(book);
                this._dbContext.SaveChanges();

                return Ok(new Models.DTO.BookResponse()
                {
                    Id = book.Id,
                    Author = book.Author,
                    Title = book.Title,
                    Category = new Models.DTO.CategoryResponse
                    {
                        Id = book.Category.Id,
                        Name = book.Category.Name
                    },
                    Year = book.Year,
                    RateAverage = book.RateAverage
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var book = this._dbContext.Books.Find(id);
                if (book == null)
                    return BadRequest("Book not found");

                this._dbContext.Books.Remove(book);
                this._dbContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/rate")]
        public ActionResult RateBook(Guid id, [FromBody] Models.DTO.RateRequest rateRequest)
        {
            try
            {
                var book = this._dbContext.Books.Find(id);
                if (book == null)
                    return BadRequest("Book not found");

                this._dbContext.Rates.Add(new Models.Domain.Rate
                {
                    Book = book,
                    Value = rateRequest.Value
                });
                this._dbContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
