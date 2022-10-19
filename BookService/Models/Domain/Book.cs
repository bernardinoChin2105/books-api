using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookService.Models.Domain
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Rate> Rates { get; set; }

        [NotMapped]
        public double RateAverage
        {
            get
            {
                return this.Rates.Count <= 0 ? 0 : this.Rates.Sum(x => x.Value) / this.Rates.Count;
            }
        }

        public Book()
        {
            this.Rates = new List<Rate>();
        }

        //public Book()
        //{
        //    this.Category = new Category();
        //    this.Rates = new List<Rate>();
        //}
    }
}