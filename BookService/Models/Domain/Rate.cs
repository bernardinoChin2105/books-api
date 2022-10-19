using System;
using System.ComponentModel.DataAnnotations;

namespace BookService.Models.Domain
{
    public class Rate
    {
        [Key]
        public Guid Id { get; set; }
        public virtual Book Book { get; set; }
        public int Value { get; set; }
    }
}