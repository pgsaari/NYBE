using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NYBE.Data;
using NYBE.Models;
using NYBE.Models.DataModels;
using Microsoft.AspNetCore.Identity;
using NYBE.Models.BookViewModels;
using Microsoft.AspNetCore.Authorization;
using NYBE.Models.AdminViewModels;
using NYBE.Services;
using Newtonsoft.Json.Linq;

namespace NYBE.Controllers
{
    public class PendingBookController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        private readonly IEmailSender _emailSender;

        public PendingBookController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            ctx = dbContext;
            usrCtx = userManager;
        }

        //GET
        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new PendingBookViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PendingBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                PendingBook pBook = new PendingBook();
                pBook.Title = viewModel.title;
                pBook.AuthorFName = viewModel.authorFName;
                pBook.AuthorLName = viewModel.authorLName;
                pBook.ISBN = viewModel.isbn;
                pBook.Edition = viewModel.edition;
                pBook.Publisher = viewModel.publisher;
                pBook.Description = viewModel.description;
                pBook.UserID = user.Id; 

                ctx.PendingBooks.Add(pBook);
                ctx.SaveChanges();

                return View("Confirm");
            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Manage()
        {
            var viewModel = new ManageBooksViewModel();

            List<PendingBook> pBooks = ctx.PendingBooks.ToList();
            List<Book> allBooks = ctx.Books.Where(a => a.Status == 1).ToList();
            List<Book> disabledBooks = ctx.Books.Where(a => a.Status == 0).ToList();
            if (pBooks != null)
            {
                viewModel.pendingBooks = pBooks;
            }
            if (allBooks != null)
            {
                viewModel.allBooks = allBooks;
            }
            if (disabledBooks != null)
            {
                viewModel.disabledBooks = disabledBooks;
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> ViewBook(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = new PendingBookViewModel();
            PendingBook book = ctx.PendingBooks.Where(a => a.ID == id).FirstOrDefault();

            // Add in search with google books api for first/best result
            HttpClient client = new HttpClient();
            string api_url = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + book.ISBN + "&key=AIzaSyAFr1FsZceoWiIHKFdSnMPeixy_ePvMG6U";
            client.BaseAddress = new Uri(api_url);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(api_url);
            if(response.IsSuccessStatusCode)
            {
                var formatters = new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter() };
                GoogleBooksAPIResponse responseObject = await response.Content.ReadAsAsync<GoogleBooksAPIResponse>(formatters);
                if(responseObject.totalItems > 0)
                {
                    viewModel.googleBook = responseObject.getBooks()[0];
                }
            }

            // make sure we found the book
            if (book != null)
            {
                viewModel.id = book.ID;
                viewModel.title = book.Title;
                viewModel.authorLName = book.AuthorLName;
                viewModel.authorFName = book.AuthorFName;
                viewModel.isbn = book.ISBN;
                viewModel.edition = book.Edition;
                viewModel.publisher = book.Publisher;
                viewModel.description = book.Description;
            }
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(PendingBookViewModel viewModel)
        {
            // remove the pending book from that table
            var removeBook = ctx.PendingBooks.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingBooks.Remove(removeBook);

            // add to the actual book table
            Book newBook = new Book();
            newBook.Title = viewModel.title;
            newBook.AuthorLName = viewModel.authorLName;
            newBook.AuthorFName = viewModel.authorFName;
            newBook.ISBN = viewModel.isbn;
            newBook.Edition = viewModel.edition;
            newBook.Publisher = viewModel.publisher;
            newBook.Description = viewModel.description;
            newBook.Status = 1;
            ctx.Books.Add(newBook);

            ctx.SaveChanges();

            // Send email notification to User that submitted the book.
            var user = await usrCtx.FindByIdAsync(removeBook.UserID);

            var callbackUrl = Url.Action("Login", "Account", null, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "NYBE Book Approval",
               $"Your recent book request for \"{removeBook.Title}\" has been approved!  <a href='{callbackUrl}'>Sign in</a> and check it out!");

            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Deny(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = new PendingBookViewModel();
            PendingBook book = ctx.PendingBooks.Where(a => a.ID == id).FirstOrDefault();

            // make sure we found the book
            if (book != null)
            {
                viewModel.id = book.ID;
                viewModel.title = book.Title;
                viewModel.authorLName = book.AuthorLName;
                viewModel.authorFName = book.AuthorFName;
                viewModel.isbn = book.ISBN;
                viewModel.edition = book.Edition;
                viewModel.publisher = book.Publisher;
                viewModel.description = book.Description;
            }
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Deny(PendingBookViewModel viewModel)
        {
            // remove the pending book from that table
            var removeBook = ctx.PendingBooks.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingBooks.Remove(removeBook);

            ctx.SaveChanges();

            // Send email notification to User that submitted the book.
            var user = await usrCtx.FindByIdAsync(removeBook.UserID);

            var callbackUrl = Url.Action("Login", "Account", null, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "NYBE Book Denial",
               $"Your recent book request for \"{removeBook.Title}\" has been denied. =[  <a href='{callbackUrl}'>Sign in</a> and try a different book!");

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Disable(PendingBookViewModel viewModel)
        {
            Book book = ctx.Books.Where(a => a.ID == viewModel.id).FirstOrDefault();
            book.Status = 0;
            ctx.SaveChanges();

            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Enable(int? id)
        {
            var viewModel = new PendingBookViewModel();
            Book book = ctx.Books.Where(a => a.ID == id).FirstOrDefault();

            // make sure we found the book
            if (book != null)
            {
                viewModel.id = book.ID;
                viewModel.title = book.Title;
                viewModel.authorLName = book.AuthorLName;
                viewModel.authorFName = book.AuthorFName;
                viewModel.isbn = book.ISBN;
                viewModel.edition = book.Edition;
                viewModel.publisher = book.Publisher;
                viewModel.description = book.Description;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Enable(PendingBookViewModel viewModel)
        {
            Book book = ctx.Books.Where(a => a.ID == viewModel.id).FirstOrDefault();
            book.Status = 1;
            ctx.SaveChanges();

            return RedirectToAction("Manage");
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return usrCtx.GetUserAsync(HttpContext.User);
        }
    }
}
