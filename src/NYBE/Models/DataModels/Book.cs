using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string AuthorLName { get; set; }
        public string AuthorFName { get; set; }
        public string Description { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string Image { get; set; }
        [Required]
        public int Status { get; set; } // 0 for inactive; 1 for active

        public ICollection<BookToCourse> BookToCourses { get; set; } // courses related to book
        // when there is a book listing with a course we can add it to the list of courses here
    }
}
