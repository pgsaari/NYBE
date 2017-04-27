using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NYBE.Models.DataModels;

namespace NYBE.Models.BookViewModels
{
    public class PendingBookViewModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "ISBN")]
        public string isbn { get; set; }

        [Required]
        [Display(Name = "title")]
        public string title { get; set; }

        [Required]
        [Display(Name = "author last name")]
        public string authorLName { get; set; }

        [Required]
        [Display(Name = "author first name")]
        public string authorFName { get; set; }

        public string description { get; set; }
        public string edition { get; set; }
        public string publisher { get; set; }
        public GoogleBook googleBook { get; set; }
        public string image { get; set; }
    }
}
