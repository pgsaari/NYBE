using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NYBE.Data;
using NYBE.Services;
using NYBE.Models;
using NYBE.Models.DataModels;
using NYBE.Models.TransactionViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NYBE.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public TransactionController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ApplicationDbContext ctx)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            var model = new BuyViewModel();
            model.listing = _context.BookListings.Where(a => a.ID == id).FirstOrDefault();
            model.book = _context.Books.Where(a => a.ID == model.listing.BookID).FirstOrDefault();
            model.seller = await _userManager.FindByIdAsync(model.listing.ApplicationUserID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(BuyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var listing = _context.BookListings.Where(a => a.ID == model.listingId).FirstOrDefault();
            if (listing != null)
            {
                var user = await GetCurrentUserAsync();
                var newLog = new TransactionLog(listing.ApplicationUserID, user.Id, listing.BookID, 0, 1, listing.AskingPrice, listing.Condition, DateTime.Now);
                _context.Add(newLog);
                await _context.SaveChangesAsync();

                // Send email notification to User that is selling book.\
                var contact = "Phone: " + user.PhoneNumber;
                if (user.PreferredContact.Equals("Email"))
                {
                    contact = "Email: " + user.Email;
                }
                var seller = await _userManager.FindByIdAsync(listing.ApplicationUserID);
                var book = _context.Books.Where(a => a.ID == listing.BookID).FirstOrDefault();
                await _emailSender.SendEmailAsync(seller.Email, "NYBE Book Interest",
                   $"User <b>{user.FirstName} {user.LastName}</b> is interested in <b>{book.Title}</b> that you have for sale!  They should be contacting you shortly, however feel free to contact them by <b>{contact}</b>.");
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Sold(int id)
        {
            var listing = _context.BookListings.Where(a => a.ID == id).FirstOrDefault();
            var model = new SoldViewModel();
            model.listingId = id;
            model.buyers = new List<ApplicationUser>();
            model.openLogs = _context.TransactionLogs.Where(a => a.SellerID == listing.ApplicationUserID && a.Status == 1 && a.BookID == listing.BookID).OrderByDescending(a => a.TransDate).ToList();
            foreach (TransactionLog tl in model.openLogs) {
                var buyer = await _userManager.FindByIdAsync(tl.BuyerID);
                model.buyers.Add(buyer);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sold(SoldViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("Buyer Id: " + model.buyerId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            return RedirectToAction(nameof(ProfileController.Index), "Profile");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
