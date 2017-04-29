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
using NYBE.Models.DataModels;

namespace NYBE.Controllers
{
    public class BookController : Controller
    {
        public const int SELL = 0;
        public const int WISHLIST = 1;
        private readonly ApplicationDbContext ctx;
        private readonly UserManager<ApplicationUser> usrCtx;
        public BookController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            ctx = dbContext;
            usrCtx = userManager;
        }

        //GET Id
        public IActionResult Index(int bookId, string sortOrder)
        {
            ViewData["PriceSortParm"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : ""; 
            ViewData["ConditionSortParm"] = sortOrder == "Condition" ? "cond_desc" : "Condition";


            var viewModel = new BookViewModel();
            var book = ctx.Books.Where(a => a.ID == bookId).FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }
            viewModel.book = book;
            //type == 0(selling) && status == 0(open)
            var forSaleListings = ctx.BookListings.Include("User").Include("User.School").Include("Book").Include("Course").Where(a => a.BookID == bookId && a.Type == 0 && a.Status == 0);
            

            switch(sortOrder)
            {
                case "cond_desc":
                    viewModel.forSaleListings = orderByCondition(forSaleListings, true);
                    break;
                case "Condition":
                    viewModel.forSaleListings = orderByCondition(forSaleListings, false);
                    break;
                case "price_desc":
                    viewModel.forSaleListings = forSaleListings.OrderByDescending(b => b.AskingPrice).ToList();
                    break;
                default:
                    viewModel.forSaleListings = forSaleListings.OrderBy(b => b.AskingPrice).ToList();
                    break;
            }

            //type == 1(buying) && status == 0(open)
            var toBuyListings = ctx.BookListings.Include("User").Include("User.School").Include("Book").Include("Course").Where(a => a.BookID == bookId && a.Type == 1 && a.Status == 0);
            viewModel.toBuyListings = toBuyListings.ToList();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Listing(int bookId)
        {
            EditListingViewModel viewModel = new EditListingViewModel();
            viewModel.book = ctx.Books.Where(a => a.ID == bookId).FirstOrDefault();
            viewModel.courses = ctx.Courses.ToList();

            return View("Listing", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Listing(EditListingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await usrCtx.GetUserAsync(HttpContext.User);
                Course newCourse = new Course();
                BookListing newListing = new BookListing();
                newListing.ApplicationUserID = user.Id;
                newListing.Condition = viewModel.condition;
                newListing.BookID = viewModel.book.ID;
                newListing.CourseID = viewModel.courseID;
                newListing.AskingPrice = viewModel.price;
                newListing.Type = SELL;
                // If they created a new course, add the course to the database.
                if (viewModel.newCourse)
                {
                    newCourse.Dept = viewModel.courseDept;
                    newCourse.CourseNum = viewModel.courseID;
                    newCourse.Name = viewModel.courseName;
                    newCourse.SchoolID = user.SchoolID;
                    ctx.Courses.Add(newCourse);
                    newListing.Course = newCourse;
                    
                } else {
                    // Connect the book and the  pre-existing course if it doesn't already exist.
                    if (!ctx.BookToCourses.Where(a => a.BookID == viewModel.book.ID && a.CourseID == viewModel.courseID).Any())
                    {
                        BookToCourse bookToCourse = new BookToCourse();
                        bookToCourse.BookID = viewModel.book.ID;
                        bookToCourse.CourseID = viewModel.courseID;
                        ctx.BookToCourses.Add(bookToCourse);
                    }
                }
                
                ctx.BookListings.Add(newListing);
                ctx.SaveChanges();

                // Put it after the new course is added and saved into the database so it receives an ID.
                if (viewModel.newCourse)
                {
                    // Connect the book and the new course if it doesn't already exist.
                    if (!ctx.BookToCourses.Where(a => a.BookID == viewModel.book.ID && a.CourseID == newCourse.ID).Any())
                    {
                        BookToCourse bookToCourse = new BookToCourse();
                        bookToCourse.BookID = viewModel.book.ID;
                        bookToCourse.CourseID = newCourse.ID;
                        ctx.BookToCourses.Add(bookToCourse);
                        ctx.SaveChanges();
                    }
                }

                return RedirectToAction("Index", "Profile");
            }
            else
            {
                viewModel.book = ctx.Books.Where(a => a.ID == viewModel.book.ID).FirstOrDefault();
                viewModel.courses = ctx.Courses.ToList();
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult WishList(int bookId)
        {
            EditListingViewModel viewModel = new EditListingViewModel();
            viewModel.book = ctx.Books.Where(a => a.ID == bookId).FirstOrDefault();
            viewModel.courses = ctx.Courses.ToList();

            return View("WishList", viewModel);
        }

        public async Task<ActionResult> WishList(EditListingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await usrCtx.GetUserAsync(HttpContext.User);
                Course newCourse = new Course();
                BookListing newListing = new BookListing();
                newListing.ApplicationUserID = user.Id;
                newListing.Condition = viewModel.condition;
                newListing.BookID = viewModel.book.ID;
                newListing.CourseID = viewModel.courseID;
                newListing.AskingPrice = viewModel.price;
                newListing.Type = WISHLIST;
                // If they created a new course, add the course to the database.
                if (viewModel.newCourse)
                {
                    newCourse.Dept = viewModel.courseDept;
                    newCourse.CourseNum = viewModel.courseID;
                    newCourse.Name = viewModel.courseName;
                    newCourse.SchoolID = user.SchoolID;
                    ctx.Courses.Add(newCourse);
                    newListing.Course = newCourse;

                }
                else
                {
                    // Connect the book and the  pre-existing course if it doesn't already exist.
                    if (!ctx.BookToCourses.Where(a => a.BookID == viewModel.book.ID && a.CourseID == viewModel.courseID).Any())
                    {
                        BookToCourse bookToCourse = new BookToCourse();
                        bookToCourse.BookID = viewModel.book.ID;
                        bookToCourse.CourseID = viewModel.courseID;
                        ctx.BookToCourses.Add(bookToCourse);
                    }
                }

                ctx.BookListings.Add(newListing);
                ctx.SaveChanges();

                // Put it after the new course is added and saved into the database so it receives an ID.
                if (viewModel.newCourse)
                {
                    // Connect the book and the new course if it doesn't already exist.
                    if (!ctx.BookToCourses.Where(a => a.BookID == viewModel.book.ID && a.CourseID == newCourse.ID).Any())
                    {
                        BookToCourse bookToCourse = new BookToCourse();
                        bookToCourse.BookID = viewModel.book.ID;
                        bookToCourse.CourseID = newCourse.ID;
                        ctx.BookToCourses.Add(bookToCourse);
                        ctx.SaveChanges();
                    }
                }

                return RedirectToAction("Index", "Profile");
            }
            else
            {
                viewModel.book = ctx.Books.Where(a => a.ID == viewModel.book.ID).FirstOrDefault();
                viewModel.courses = ctx.Courses.ToList();
                return View(viewModel);
            }
        }

        private List<BookListing> orderByCondition(IQueryable<BookListing> listings, bool isDescending)
        {
            List<BookListing> newListing = listings.Where(l => l.Condition == "New").ToList();
            List<BookListing> excellentListing = listings.Where(l => l.Condition == "Excellent").ToList();
            List<BookListing> goodListing = listings.Where(l => l.Condition == "Good").ToList();
            List<BookListing> fairListing = listings.Where(l => l.Condition == "Fair").ToList();
            List<BookListing> badListing = listings.Where(l => l.Condition == "Bad").ToList();

            if(isDescending) {
                newListing.AddRange(excellentListing);
                newListing.AddRange(goodListing);
                newListing.AddRange(fairListing);
                newListing.AddRange(badListing);
                return newListing;
            }

            else
            {
                badListing.AddRange(fairListing);
                badListing.AddRange(goodListing);
                badListing.AddRange(excellentListing);
                badListing.AddRange(newListing);
                return badListing;
            }
        }
    }  
}