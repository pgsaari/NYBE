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
    }
}