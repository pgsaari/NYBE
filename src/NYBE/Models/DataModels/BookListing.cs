using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class BookListing
    {
        public BookListing()
        {

        }

        public BookListing(int bookID, string userID, int courseID, string cond, double price, int type)
        {
            this.BookID = bookID;
            ApplicationUserID = userID;
            CourseID = courseID;
            Condition = cond;
            AskingPrice = price;
            Type = type;
        }

        public int ID { get; set; }
        public int BookID { get; set; }
        public string ApplicationUserID { get; set; }
        public int CourseID { get; set; }
        public string Condition { get; set; }
        public int Type { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double AskingPrice { get; set; }

        public Book Book { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
    }
}
