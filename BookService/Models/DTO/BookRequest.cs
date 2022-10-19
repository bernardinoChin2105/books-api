using System;

namespace BookService.Models.DTO
{
    public class BookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public Guid Category { get; set; }
    }
}
