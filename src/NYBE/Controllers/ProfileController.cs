using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var view = new ProfileViewModel();
            var user = await GetCurrentUserAsync();

            view.isAdmin = await usrCtx.IsInRoleAsync(user, "Admin");
            view.name = user.FirstName + " " + user.LastName;
            view.email = user.Email;
            view.phone = user.PhoneNumber;
            view.rating = user.Rating;
            view.school = ctx.Schools.Where(a => a.ID == user.SchoolID).FirstOrDefault();

            // get all the user's book listings include the book and course objects to view in the table
            view.listings = ctx.BookListings.Include("Book").Include("Course").Where(a => user.Id == a.ApplicationUserID).ToList();

            // get the transaction history for this user
            view.transactions = ctx.TransactionLogs.Include("Seller").Include("Buyer").Include("Book").
                //Where(a => (user.Id == a.BuyerID) || (user.Id == a.SellerID)).
                OrderBy(a => a.TransDate).ToList();

            return View(view);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }
    }
}
