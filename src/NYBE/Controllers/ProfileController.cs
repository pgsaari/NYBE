using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NYBE.Data;
using NYBE.Models;
using NYBE.Models.DataModels;
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var view = await loadProfile();

            return View(view);
        }

        [HttpGet]
        public ActionResult EditListing(int id)
        {
            var viewModel = new EditListingViewModel();
            var temp = ctx.BookListings.Include("Book").Include("Course").Where(a => a.ID == id).FirstOrDefault();
            viewModel.ID = id;
            viewModel.book = temp.Book;
            viewModel.course = temp.Course;
            viewModel.condition = temp.Condition;
            viewModel.price = temp.AskingPrice;
            return View("EditListing", viewModel);
        }

        [HttpPost]
        public ActionResult EditListing(EditListingViewModel viewModel)
        {
            var temp = ctx.BookListings.Where(a => a.ID == viewModel.ID).FirstOrDefault();
            temp.Condition = viewModel.condition;
            temp.AskingPrice = viewModel.price;
            ctx.Update(temp);
            var success = ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteListing(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = new EditListingViewModel();
            var temp = ctx.BookListings.Include("Book").Include("Course").Where(a => a.ID == id).FirstOrDefault();
            if (temp == null) return NotFound();
            viewModel.ID = temp.ID;
            viewModel.book = temp.Book;
            viewModel.course = temp.Course;
            viewModel.condition = temp.Condition;
            viewModel.price = temp.AskingPrice;
            return View("DeleteListing", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteListing(EditListingViewModel viewModel)
        {
            var temp = ctx.BookListings.Where(a => a.ID == viewModel.ID).FirstOrDefault();
            ctx.BookListings.Remove(temp);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }

        private async Task<ProfileViewModel> loadProfile()
        {
            ProfileViewModel view = new ProfileViewModel();
            var user = await GetCurrentUserAsync();

            view.isAdmin = await usrCtx.IsInRoleAsync(user, "Admin");
            view.name = user.FirstName + " " + user.LastName;
            view.email = user.Email;
            view.phone = user.PhoneNumber;
            view.rating = user.Rating;

            switch (user.PreferredContact)
            {
                case 0:
                    view.preferredContact = "Email";
                    break;
                case 1:
                    view.preferredContact = "Call";
                    break;
                case 2:
                    view.preferredContact = "Text";
                    break;
            }

            view.school = ctx.Schools.Where(a => a.ID == user.SchoolID).FirstOrDefault();

            // get all the user's book listings include the book and course objects to view in the table
            view.listings = ctx.BookListings.Include("Book").Include("Course").Where(a => user.Id == a.ApplicationUserID).ToList();


            // get the sold transaction history for this user
            view.soldTransactions = ctx.TransactionLogs.Include("Buyer").Include("Book").
                Where(a => user.Id == a.SellerID).
                OrderByDescending(a => a.TransDate).ToList();

            // get the bought transaction history for this user
            view.boughtTransactions = ctx.TransactionLogs.Include("Seller").Include("Book").
                Where(a => user.Id == a.BuyerID).
                OrderByDescending(a => a.TransDate).ToList();
            
            view.wishList = ctx.BookListings.Include("User").Include("User.School").Include("Book").Include("Course").Where(a => user.Id==a.ApplicationUserID && a.Type == 1).ToList();

            return view;
        }
    }
}
