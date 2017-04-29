using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.DataModels
{
    public class EditListingViewModel
    {
        public int ID { get; set; }
        [Required]
        public string condition { get; set; }
        [Required]
        public double price { get; set; }
        public Book book { get; set; }
        
        public Course course { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The course field is required.")]
        public int courseID { get; set; }
        public List<Course> courses {get; set;}

        [Required]
        [Display(Name = "department")]
        public string courseDept { get; set; }
        //[Required]
        //[Display(Name = "course number")]
        //public string courseNum { get; set; }
        [Required]
        [Display(Name = "course name")]
        public string courseName { get; set; }
        public bool newCourse { get; set; }
    }
}
