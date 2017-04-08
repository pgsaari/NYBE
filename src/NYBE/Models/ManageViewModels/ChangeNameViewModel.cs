using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;

namespace NYBE.Models.ManageViewModels
{
    public class ChangeNameViewModel
    {
        public string OldFirstName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public string OldLastName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
