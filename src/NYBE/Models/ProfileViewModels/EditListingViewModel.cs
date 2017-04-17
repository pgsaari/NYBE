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
        [Range(1,int.MaxValue, ErrorMessage = "Error in choosing Course")]
        public int courseID { get; set; }
        public List<Course> courses {get; set;}
    }
}
