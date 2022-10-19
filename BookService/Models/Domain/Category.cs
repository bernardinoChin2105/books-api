using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookService.Models.Domain
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}