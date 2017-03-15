using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NYBE.Data;
using NYBE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NYBE.Models.AdminViewModels;
using Microsoft.EntityFrameworkCore;

namespace NYBE.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            ctx = dbContext;
            usrCtx = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();
            ManageUsersViewModel viewModel = new ManageUsersViewModel();
            viewModel.roles = new List<string>();
            viewModel.users = ctx.Users.Include("Roles").Include("School").Where(a => a.Id != currentUser.Id).ToList(); // get all users not including current user
            string roleName = "";
            // get the role names separately because bill gates hates me
            foreach(var user in viewModel.users)
            {
                var role = await usrCtx.GetRolesAsync(user);
                roleName = role.FirstOrDefault();
                viewModel.roles.Add(roleName);
            }

            return View(viewModel);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }
    }
}