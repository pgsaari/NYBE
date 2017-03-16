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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();
            ManageUsersViewModel viewModel = new ManageUsersViewModel();
            viewModel.roles = new List<string>();
            viewModel.users = ctx.Users.Include("Roles").Include("School").Where(a => a.Id != currentUser.Id && a.Status == 1).ToList(); // get all active users not including current user
            string roleName = "";

            // get the role names separately because bill gates hates me
            foreach(var user in viewModel.users)
            {
                var role = await usrCtx.GetRolesAsync(user);
                roleName = role.FirstOrDefault();
                viewModel.roles.Add(roleName);
            }

            viewModel.disabledUsers = ctx.Users.Include("School").Where(a => a.Status == 0).ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            var viewModel = new EditUserViewModel();
            ApplicationUser user = ctx.Users.Include("Roles").Include("School").Where(a => a.Id == id).FirstOrDefault();
            viewModel.user = user;
            viewModel.name = user.FirstName + " " +user.LastName;
            viewModel.email = user.Email;
            viewModel.phone = user.PhoneNumber;
            viewModel.rating = user.Rating;
            viewModel.userId = user.Id;
            viewModel.school = user.School.Name;

            var tempRole = await usrCtx.GetRolesAsync(user);
            viewModel.oldRole = tempRole.FirstOrDefault();

            return View("EditUser", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(EditUserViewModel model )
        {
            if (ModelState.IsValid)
            {
                var user = await usrCtx.FindByIdAsync(model.userId);

                // remove user from old role and add to new role
                await usrCtx.RemoveFromRoleAsync(user, model.oldRole);
                await usrCtx.AddToRoleAsync(user, model.newRole);


                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var viewModel = new EditUserViewModel();
            ApplicationUser user = ctx.Users.Include("Roles").Include("School").Where(a => a.Id == id).FirstOrDefault();
            viewModel.user = user;
            viewModel.name = user.FirstName + " " + user.LastName;
            viewModel.email = user.Email;
            viewModel.phone = user.PhoneNumber;
            viewModel.rating = user.Rating;
            viewModel.userId = user.Id;
            viewModel.school = user.School.Name;

            var tempRole = await usrCtx.GetRolesAsync(user);
            viewModel.oldRole = tempRole.FirstOrDefault();

            return View("DeleteUser", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = ctx.Users.Where(a => a.Id == model.userId).FirstOrDefault();
                user.Status = 0; // inactivate this user
                await ctx.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var viewModel = new EditUserViewModel();
            ApplicationUser user = ctx.Users.Include("Roles").Include("School").Where(a => a.Id == id).FirstOrDefault();
            viewModel.user = user;
            viewModel.name = user.FirstName + " " + user.LastName;
            viewModel.email = user.Email;
            viewModel.phone = user.PhoneNumber;
            viewModel.rating = user.Rating;
            viewModel.userId = user.Id;
            viewModel.school = user.School.Name;

            var tempRole = await usrCtx.GetRolesAsync(user);
            viewModel.oldRole = tempRole.FirstOrDefault();

            return View("ActivateUser", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = ctx.Users.Where(a => a.Id == model.userId).FirstOrDefault();
                user.Status = 1; // activate this user
                await ctx.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }
    }
}