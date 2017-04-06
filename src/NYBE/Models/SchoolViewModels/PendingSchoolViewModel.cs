using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.SchoolViewModels
{
    public class PendingSchoolViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
    }
}
