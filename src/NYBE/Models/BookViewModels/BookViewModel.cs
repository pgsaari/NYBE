using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.BookViewModel
{
    public class BookViewModel
    {
        public Book book { get; set; }
        public List<BookListing> forSaleListings { get; set; }
        public List<BookListing> toBuyListings { get; set; }
    }
}
