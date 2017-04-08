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

namespace NYBE.Controllers
{
    public class PendingSchoolController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        public PendingSchoolController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
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
        public IActionResult Index(PendingSchoolViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PendingSchool pSchool = new PendingSchool();
                pSchool.Name = viewModel.name;
                pSchool.City = viewModel.city;
                pSchool.State = viewModel.state;

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
            if (pSchools != null)
            {
                viewModel.pendingSchools = pSchools;
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
        public IActionResult Approve(PendingSchoolViewModel viewModel)
        {
            // remove the pending book from that table
            var removeSchool = ctx.PendingSchools.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingSchools.Remove(removeSchool);

            // add to the actual book table
            School newSchool = new School();
            newSchool.Name = viewModel.name;
            newSchool.City = viewModel.city;
            newSchool.State = viewModel.state;
            ctx.Schools.Add(newSchool);

            ctx.SaveChanges();

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
        public IActionResult Deny(PendingSchoolViewModel viewModel)
        {
            // remove the pending school from that table
            var removeSchool = ctx.PendingSchools.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingSchools.Remove(removeSchool);

            ctx.SaveChanges();

            return RedirectToAction("Manage");
        }
    }
}