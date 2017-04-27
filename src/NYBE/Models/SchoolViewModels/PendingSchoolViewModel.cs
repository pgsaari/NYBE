using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.SchoolViewModels
{
    public class PendingSchoolViewModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "city")]
        public string city { get; set; }

        [Required]
        [Display(Name = "state")]
        public string state { get; set; }
    }
}
