using System;

namespace BookService.Models.DTO
{
    public class BookResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public CategoryResponse Category { get; set; }
        public double RateAverage { get; set; }
    }
}
