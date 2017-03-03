using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class BookListing
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public int ApplicationUserID { get; set; }
        public int CourseID { get; set; }
        public string Condition { get; set; }
        public double AskingPrice { get; set; }

        public Book Book { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
    }
}
