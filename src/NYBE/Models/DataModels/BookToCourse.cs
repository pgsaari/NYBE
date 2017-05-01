using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class BookToCourse
    {
        public BookToCourse() { }

        public BookToCourse(int bookID, int courseID)
        {
            BookID = bookID;
            CourseID = courseID;
        }

        public int ID { get; set; }
        public int BookID { get; set; }
        public int CourseID { get; set; }

        public Book Book { get; set; }
        public Course Course { get; set; }
    }
}
