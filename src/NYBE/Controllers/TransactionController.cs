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
            var listing = model.listing;
            if (listing != null)
            {
                var user = await GetCurrentUserAsync();
                var newLog = new TransactionLog(listing.ApplicationUserID, user.Id, listing.BookID, 0, 0, listing.AskingPrice, listing.Condition, DateTime.Now);
                //_context.Add(newLog);
                //await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
