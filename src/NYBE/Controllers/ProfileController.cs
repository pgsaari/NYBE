using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NYBE.Data;
using NYBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Controllers
{
    public class ProfileController: Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            ctx = dbContext;
            usrCtx = userManager;
        }
        // 
        // GET: /Profile/

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            ViewData["UserName"] = user.UserName; // TODO: make this an actual user name instead of email address
            ViewData["Rating"] = user.Rating;
            var book = ctx.Books.Where(a => a.ID == 2).FirstOrDefault();
            ViewData["Book"] = book.Title + " by " + book.AuthorFName + " " + book.AuthorLName;

            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }
    }
}
