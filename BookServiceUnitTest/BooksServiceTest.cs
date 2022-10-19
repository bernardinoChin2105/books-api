using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookServiceUnitTest
{
    [TestClass]
    public class BooksServiceTest
    {
        private BookService.Data.BooksDbContext context;

        public BooksServiceTest()
        {
            this.InitializeContext();
        }

        private void InitializeContext()
        {
            var builder = new DbContextOptionsBuilder<BookService.Data.BooksDbContext>().UseInMemoryDatabase("BooksDB");
            this.context = new BookService.Data.BooksDbContext(builder.Options);
            this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "Literatura" });
            this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "Historia" });
            this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "Tecnología" });
            this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "Ciencia" });
            this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "Cocina" });
        }

        [TestMethod]
        public void GET_Retrieve_All_Categories()
        {            
            var controller = new BookService.Controllers.CategoriesController(this.context);
            var response = controller.Get();            

            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
            Assert.IsTrue(((OkObjectResult)response.Result).StatusCode == 200);
            Assert.IsInstanceOfType(((OkObjectResult)response.Result).Value, typeof(IEnumerable<BookService.Models.DTO.CategoryResponse>));
            Assert.IsTrue(((IEnumerable<BookService.Models.DTO.CategoryResponse>)((OkObjectResult)response.Result).Value != null));
        }

        [TestMethod]
        public void POST_Insert_Book()
        {

            var category = this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "TestCategory" });
            var controller = new BookService.Controllers.BooksController(this.context);

            var response = controller.Post(new BookService.Models.DTO.BookRequest
            {
                Title = "La ciudad de las bestias",
                Author = "Isabel Allende",
                Category = category.Entity.Id,
                Year = 2017
            });

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsTrue(((OkObjectResult)response).StatusCode == 200);
            Assert.IsInstanceOfType(((OkObjectResult)response).Value, typeof(BookService.Models.DTO.BookResponse));
            Assert.IsTrue(((BookService.Models.DTO.BookResponse)((OkObjectResult)response).Value).Id != null);
            Assert.IsTrue(!string.IsNullOrEmpty(((BookService.Models.DTO.BookResponse)((OkObjectResult)response).Value).Title));
            Assert.IsTrue(!string.IsNullOrEmpty(((BookService.Models.DTO.BookResponse)((OkObjectResult)response).Value).Author));
            Assert.IsTrue(((BookService.Models.DTO.BookResponse)((OkObjectResult)response).Value).Year != int.MinValue);
            Assert.IsTrue(((BookService.Models.DTO.BookResponse)((OkObjectResult)response).Value).Category.Id != null);
        }

        [TestMethod]
        public void POST_Rate_Book()
        {

            var category = this.context.Categories.Add(new BookService.Models.Domain.Category() { Id = Guid.NewGuid(), Name = "TestCategory" });
            var book = this.context.Books.Add(new BookService.Models.Domain.Book
            {
                Title = "La ciudad de las bestias",
                Author = "Isabel Allende",
                Category = category.Entity,
                Year = 2017
            });


            var controller = new BookService.Controllers.BooksController(this.context);
            var response = controller.RateBook(book.Entity.Id, new BookService.Models.DTO.RateRequest
            {
                Value = 5
            });

            Assert.IsInstanceOfType(response, typeof(OkResult));
            Assert.IsTrue(((OkResult)response).StatusCode == 200);
        }
    }
}
