using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.ManageViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm new email")]
        [Compare("NewEmail", ErrorMessage = "The new email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }
    }
}
