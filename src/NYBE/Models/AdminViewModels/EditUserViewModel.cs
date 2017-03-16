using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.AdminViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser user { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public double rating { get; set; }
        public string school { get; set; }
        public string userId { get; set; }
        public string oldRole { get; set; }
        public string newRole { get; set; }


    }
}
