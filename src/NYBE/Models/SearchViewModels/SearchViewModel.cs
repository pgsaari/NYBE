using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

// Reference: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search

namespace NYBE.Models.SearchViewModels
{
    public class BookSearchViewModel
    {
        public List<Book> bookList;
        public List<School> schools;
        public List<Course> courses;
        public string schoolSelected { get; set; }
        public string courseSelected { get; set; }
    }
}
