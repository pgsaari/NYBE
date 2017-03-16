using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.AdminViewModels
{
    public class ManageUsersViewModel
    {

        public List<ApplicationUser> users { get; set; }
        public List<string> roles { get; set; }
        public List<ApplicationUser> disabledUsers { get; set; }
    }
}
