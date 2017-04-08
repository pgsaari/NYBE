using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NYBE.Models;
using NYBE.Models.SearchViewModels;

// Resource: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search

namespace NYBE.Controllers
{
    public class SearchController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public SearchController(Data.ApplicationDbContext context)
        {
            _context = context;
        }


        //
        // GET: /search/
        public IActionResult Index(string generalSearch, string title, string author, string isbn, string courseName)
        {
            var bookList = new List<Book>();

            //chars to split by
            char[] delimiterChars = { ' ', ',', '.', ':' };

            if(!String.IsNullOrEmpty(generalSearch))
            {
                string[] words = generalSearch.Split(delimiterChars);
                foreach (string str in words)
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        bookList = bookList.Union(getBooksByGeneralString(str)).ToList();
                    }
                }
            }
            
            
            // commented out to be used for advanced search
            if (!String.IsNullOrEmpty(title))
            {
                bookList = getBooksByTitle(title, bookList);
            }
            if (!String.IsNullOrEmpty(author))
            {
                bookList = getBooksByAuthor(author, bookList);
            }
            if (!String.IsNullOrEmpty(isbn))
            {
                bookList = getBooksByIsbn(isbn, bookList);
            }
            if (!String.IsNullOrEmpty(courseName))
            {
                bookList = getBooksByCourse(courseName, bookList);
            }
            
            


            var bookSearchViewModel = new BookSearchViewModel();
            //Switch Where() with courses for the school = User.school
            // bookSearchViewModel.courses = _context.Courses.Where(school => ).ToList();
            bookSearchViewModel.courses = _context.Courses.ToList();
            bookSearchViewModel.bookList = bookList;

            return View(bookSearchViewModel);
        }

        public List<Book> getBooksByGeneralString(string searchTerm)
        {
            List<Book> books = _context.Books.Where(book => book.AuthorFName.Contains(searchTerm) || book.AuthorLName.Contains(searchTerm) || book.Title.Contains(searchTerm) || book.ISBN.Equals(searchTerm)).ToList();
            return books;
        }

        /* Returns List<Book> where name is contained in first or last name of author.
         * If List is Empty it will query the database, else
         * it will filter the list by name */
        public List<Book> getBooksByAuthor(string name, List<Book> listToFilter)
        {
            if (listToFilter.Count == 0)
                return _context.Books.Where(book => book.AuthorLName.Contains(name) || book.AuthorFName.Contains(name)).ToList();
            else
                return listToFilter.Where(book => book.AuthorLName.Contains(name) || book.AuthorFName.Contains(name)).ToList();
        }

        /* Returns List<Book> where title is contained in book title.
         * If List is Empty it will query the database, else
         * it will filter the list by title */
        public List<Book> getBooksByTitle(string title, List<Book> listToFilter)
        {
            if (listToFilter.Count == 0)
                return _context.Books.Where(book => book.Title.Contains(title)).ToList();
            else
                return listToFilter.Where(book => book.Title.Contains(title)).ToList();
        }

        /* Returns List<Book> where isbn is matched.
         * If List is Empty it will query the database, else
         * it will filter the list by isbn matches */
        public List<Book> getBooksByIsbn(string isbn, List<Book> listToFilter)
        {
            if (listToFilter.Count == 0)
                return _context.Books.Where(book => book.ISBN.Equals(isbn)).ToList();
            else
                return listToFilter.Where(book => book.ISBN.Equals(isbn)).ToList();
        }

        
        public List<Book> getBooksByCourse(string courseName, List<Book> listToFilter)
        {
            Course selectedCourse = _context.Courses.Where(c => c.Name.Equals(courseName)).ToList()[0];
            List<BookToCourse> bookIDs = _context.BookToCourses.Where(item => item.CourseID.Equals(selectedCourse.ID)).ToList();

            if (listToFilter.Count == 0)
                return _context.Books.Where(book => bookIDs.Exists(item => item.BookID == book.ID)).ToList();
            else
                return listToFilter.Where(book => bookIDs.Exists(item => item.BookID == book.ID)).ToList();
        }
    }
}
