using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NYBE.Models.ManageViewModels
{
    public class ChangeSchoolViewModel
    {
        public List<School> Schools { get; set; }

        public int OldSchool { get; set; }

        [Required]
        [Display(Name = "School")]
        public int School { get; set; }
    }
}
