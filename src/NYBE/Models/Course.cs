using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Dept { get; set; } // ex. CS
        public string Name { get; set; } // ex. Capstone Project
        public int CourseNum { get; set; } // ex. 595
        public int SchoolID { get; set; }

        public School School { get; set; }
        public ICollection<BookToCourse> BookToCourses { get; set; } // a course can have many books
        // this is going to make a M:M relationship between Book and Course
    }
}
