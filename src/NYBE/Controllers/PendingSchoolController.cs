using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NYBE.Data;
using NYBE.Models;
using Microsoft.AspNetCore.Identity;
using NYBE.Models.BookViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NYBE.Models.AdminViewModels;
using NYBE.Models.SchoolViewModels;
using NYBE.Services;

namespace NYBE.Controllers
{
    public class PendingSchoolController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        private readonly IEmailSender _emailSender;
        public PendingSchoolController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            ctx = dbContext;
            usrCtx = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new PendingSchoolViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PendingSchoolViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                PendingSchool pSchool = new PendingSchool();
                pSchool.Name = viewModel.name;
                pSchool.City = viewModel.city;
                pSchool.State = viewModel.state;
                pSchool.UserID = user.Id;

                ctx.PendingSchools.Add(pSchool);
                ctx.SaveChanges();

                return View("Confirm");
            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            var viewModel = new ManageSchoolsViewModel();

            List<PendingSchool> pSchools = ctx.PendingSchools.ToList();
            List<School> allSchools = ctx.Schools.Where(a => a.Status == 1).ToList();
            List<School> disabledSchools = ctx.Schools.Where(a => a.Status == 0).ToList();
            if (pSchools != null)
            {
                viewModel.pendingSchools = pSchools;
            }

            if (allSchools != null)
            {
                viewModel.allSchools = allSchools;
            }

            if(disabledSchools != null)
            {
                viewModel.disabledSchools = disabledSchools;
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = new PendingSchoolViewModel();
            PendingSchool pSchool = ctx.PendingSchools.Where(a => a.ID == id).FirstOrDefault();

            // make sure we found the school
            if (pSchool != null)
            {
                viewModel.id = pSchool.ID;
                viewModel.name = pSchool.Name;
                viewModel.city = pSchool.City;
                viewModel.state = pSchool.State;
            }
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(PendingSchoolViewModel viewModel)
        {
            // remove the pending book from that table
            var removeSchool = ctx.PendingSchools.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingSchools.Remove(removeSchool);

            // add to the actual book table
            School newSchool = new School();
            newSchool.Name = viewModel.name;
            newSchool.City = viewModel.city;
            newSchool.State = viewModel.state;
            newSchool.Status = 1;
            ctx.Schools.Add(newSchool);

            ctx.SaveChanges();

            // Send email notification to User that submitted the school.
            var user = await usrCtx.FindByIdAsync(removeSchool.UserID);

            var callbackUrl = Url.Action("Login", "Account", null, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "NYBE School Approval",
               $"Your recent school request for \"{removeSchool.Name}\" has been approved!  <a href='{callbackUrl}'>Sign in</a> and check it out!");

            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Deny(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = new PendingSchoolViewModel();
            PendingSchool pSchool = ctx.PendingSchools.Where(a => a.ID == id).FirstOrDefault();

            // make sure we found the school
            if (pSchool != null)
            {
                viewModel.id = pSchool.ID;
                viewModel.name = pSchool.Name;
                viewModel.city = pSchool.City;
                viewModel.state = pSchool.State;
            }
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deny(PendingSchoolViewModel viewModel)
        {
            // remove the pending school from that table
            var removeSchool = ctx.PendingSchools.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingSchools.Remove(removeSchool);

            ctx.SaveChanges();

            // Send email notification to User that submitted the school.
            var user = await usrCtx.FindByIdAsync(removeSchool.UserID);

            var callbackUrl = Url.Action("Login", "Account", null, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "NYBE School Denial",
               $"Your recent school request for \"{removeSchool.Name}\" has been denied. =[  <a href='{callbackUrl}'>Sign in</a> and try a different school!");

            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Disable(int? id)
        {
            PendingSchoolViewModel viewModel = new PendingSchoolViewModel();
            School school = ctx.Schools.Where(a => a.ID == id).FirstOrDefault();
            viewModel.id = school.ID;
            viewModel.name = school.Name;
            viewModel.city = school.City;
            viewModel.state = school.State;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Disable(PendingSchoolViewModel viewModel)
        {
            School school = ctx.Schools.Where(a => a.ID == viewModel.id).FirstOrDefault();
            school.Status = 0;
            ctx.SaveChanges();

            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Enable(int? id)
        {
            PendingSchoolViewModel viewModel = new PendingSchoolViewModel();
            School school = ctx.Schools.Where(a => a.ID == id).FirstOrDefault();
            viewModel.id = school.ID;
            viewModel.name = school.Name;
            viewModel.city = school.City;
            viewModel.state = school.State;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Enable(PendingSchoolViewModel viewModel)
        {
            School school = ctx.Schools.Where(a => a.ID == viewModel.id).FirstOrDefault();
            school.Status = 1;
            ctx.SaveChanges();

            return RedirectToAction("Manage");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }
    }
}