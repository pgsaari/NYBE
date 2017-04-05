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
using NYBE.Models.BookViewModels;
using Microsoft.AspNetCore.Authorization;
using NYBE.Models.AdminViewModels;

namespace NYBE.Controllers
{
    public class PendingBookController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        public PendingBookController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
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
        public IActionResult Index(PendingBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PendingBook pBook = new PendingBook();
                pBook.Title = viewModel.title;
                pBook.AuthorFName = viewModel.authorFName;
                pBook.AuthorLName = viewModel.authorLName;
                pBook.ISBN = viewModel.isbn;
                pBook.Edition = viewModel.edition;
                pBook.Publisher = viewModel.publisher;
                pBook.Description = viewModel.description;

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

            List<PendingBook> books = ctx.PendingBooks.ToList();
            if(books != null)
            {
                viewModel.pendingBooks = books;
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = new PendingBookViewModel();
            PendingBook book = ctx.PendingBooks.Where(a => a.ID == id).FirstOrDefault();

            // make sure we found the book
            if(book != null)
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
        public IActionResult Approve(PendingBookViewModel viewModel)
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
            ctx.Books.Add(newBook);

            ctx.SaveChanges();

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
        public IActionResult Deny(PendingBookViewModel viewModel)
        {
            // remove the pending book from that table
            var removeBook = ctx.PendingBooks.Where(a => a.ID == viewModel.id).FirstOrDefault();
            ctx.PendingBooks.Remove(removeBook);

            ctx.SaveChanges();

            return RedirectToAction("Manage");
        }

    }
}
