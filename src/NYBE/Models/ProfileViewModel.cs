using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

// Reference: https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search

namespace NYBE.Models
{
    public class ProfileViewModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public double rating { get; set; }
        public School school { get; set; }
        public bool isAdmin { get; set; }
        public List<BookListing> listings { get; set; }
        public List<TransactionLog> transactions { get; set; }

    }
}
