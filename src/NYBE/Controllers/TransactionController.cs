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
        public async Task<IActionResult> Index(int id)
        {
            var log = _context.TransactionLogs.Where(a => a.ID == id).FirstOrDefault();
            var buyer = await _userManager.FindByIdAsync(log.BuyerID);
            var seller = await _userManager.FindByIdAsync(log.SellerID);
            if (log == null || buyer == null || seller == null) {
                return View("Error");
            }
            var book = _context.Books.Where(a => a.ID == log.BookID).FirstOrDefault();
            if (book == null) {
                return View("Error");
            }
            var notes = log.Comments;
            if (log.Comments == null || log.Comments.Length < 1) { notes = "None"; }
            var model = new IndexViewModel {
                TransRating = log.TransRating,
                SoldPrice = log.SoldPrice,
                Condition = log.Condition,
                Comments = notes,
                Date = log.TransDate,
                Book = book,
                Seller = seller,
                Buyer = buyer
            };
            return View(model);
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
        public async Task<IActionResult> Cancel(int id)
        {
            var openLog = _context.TransactionLogs.Where(a => a.ID == id).FirstOrDefault();
            if (openLog != null)
            {
                var listing = _context.BookListings.Where(a => a.ApplicationUserID == openLog.SellerID && a.BookID == openLog.BookID && a.AskingPrice == openLog.SoldPrice && a.Condition == openLog.Condition).FirstOrDefault();
                // Restore old listing and remove pending transaction
                listing.Status = 0;
                _context.TransactionLogs.Remove(openLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ProfileController.Index), "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Survey(int id = -1, string token = null, string buyerId = null)
        {
            System.Diagnostics.Debug.WriteLine("ID: " + id);
            System.Diagnostics.Debug.WriteLine("Token: " + token);

            if (token == null) return View("Error");
            var listing = _context.BookListings.Where(a => a.ID == id && (token.Equals("owner") ? a.Status == 0 : a.Status == 1)).FirstOrDefault();
            if (listing == null)
            {
                return View("Expired");
            }
            var model = new SurveyViewModel();
            model.token = token;
            model.listing = listing;
            model.listingId = id;
            model.courses = _context.Courses.ToList();
            model.condition = listing.Condition;
            model.buyers = new List<ApplicationUser>();
            if (token.Equals("owner"))
            {
                model.openLogs = _context.TransactionLogs.Where(a => a.SellerID == listing.ApplicationUserID && a.Status == 1 && a.BookID == listing.BookID).OrderByDescending(a => a.TransDate).ToList();
                foreach (TransactionLog tl in model.openLogs)
                {
                    var buyer = await _userManager.FindByIdAsync(tl.BuyerID);
                    model.buyers.Add(buyer);
                }
            } else
            {
                byte[] data = Convert.FromBase64String(model.token);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                //if (when < DateTime.UtcNow.AddHours(-48))
                //{
                //    // TODO: Restore old listing and remove pending transaction
                //    return View("Expired");
                //}
                var buyer = await _userManager.FindByIdAsync(buyerId);
                model.buyers.Add(buyer);
                model.buyerId = buyerId;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Survey(SurveyViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("Buyer: " + model.buyer);
            System.Diagnostics.Debug.WriteLine("Canceled: " + model.canceled);
            if (model.buyer)
            {
                byte[] data = Convert.FromBase64String(model.token);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                var logid = BitConverter.ToInt32(data, 8);
                var log = _context.TransactionLogs.Where(a => a.ID == logid).FirstOrDefault();
                var listing = _context.BookListings.Where(a => a.ID == model.listingId).FirstOrDefault();
                if (model.canceled)
                {
                    // Restore old listing and remove pending transaction
                    listing.Status = 0;
                    _context.TransactionLogs.Remove(log);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // If the condition or the price is different, discard this transaction and book listing.
                    _context.BookListings.Remove(listing);
                    if (log.Condition != model.condition || log.SoldPrice != model.finalPrice)
                    {
                        _context.TransactionLogs.Remove(log);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Otherwise remove the pending book listing, close the transaction, and update seller's rating.
                        log.Status = 0;
                        log.TransRating = model.rating;
                        log.Comments = model.comments;
                        var seller = await _userManager.FindByIdAsync(log.SellerID);
                        var sellerLogs = _context.TransactionLogs.Where(a => a.SellerID == log.SellerID && a.Status == 0).ToList();
                        var newRating = model.rating;
                        foreach (TransactionLog tl in sellerLogs)
                        {
                            newRating += tl.TransRating;
                        }
                        seller.Rating = newRating / (sellerLogs.Count + 1);
                        await _context.SaveChangesAsync();
                    }
                }
                return View("Complete");
            }
            else
            {
                var listing = _context.BookListings.Where(a => a.ID == model.listingId).FirstOrDefault();
                listing.CourseID = model.courseID;
                listing.Condition = model.condition;
                listing.AskingPrice = model.finalPrice;
                listing.Status = 1;
                var log = _context.TransactionLogs.Where(a => a.SellerID == listing.ApplicationUserID && a.BuyerID == model.buyerId && a.BookID == listing.BookID && a.Status == 1).FirstOrDefault();
                log.Condition = model.condition;
                log.SoldPrice = model.finalPrice;
                await _context.SaveChangesAsync();
                // Send email to buyer and notify them of the sell with link to fill out the survey
                var seller = await _userManager.FindByIdAsync(listing.ApplicationUserID);
                var buyer = await _userManager.FindByIdAsync(model.buyerId);
                var book = _context.Books.Where(a => a.ID == listing.BookID).FirstOrDefault();
                System.Diagnostics.Debug.WriteLine("LogId: " + log.ID);

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] logid = BitConverter.GetBytes(log.ID);
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(logid).Concat(key).ToArray());
                var callbackUrl = Url.Action("Survey", "Transaction", new { id = listing.ID, token = token, buyerId = buyer.Id }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(buyer.Email, "NYBE Complete Purchase",
                   $"<b>{seller.FirstName} {seller.LastName}</b> has marked you as the purchaser of <b>{book.Title}</b>. Please fill out our <a href='{callbackUrl}'>survey</a> to confirm your purchase and leave a rating.  If you have not purchased this book, you can say so in the survey.  You should hurry!  The survey expires in 48 hours from receiving this email!");
                return RedirectToAction(nameof(ProfileController.Index), "Profile");
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
