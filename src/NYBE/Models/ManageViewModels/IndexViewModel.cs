using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NYBE.Models.ManageViewModels
{
    public class IndexViewModel
    {
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public School School { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
