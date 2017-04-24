using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.TransactionViewModels
{
    public class BuyViewModel
    {
        public int listingId { get; set; }

        public BookListing listing { get; set; }
        public Book book { get; set; }
        public ApplicationUser seller { get; set; }
    }
}
