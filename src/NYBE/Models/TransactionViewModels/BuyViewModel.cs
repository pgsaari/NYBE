using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.TransactionViewModels
{
    public class BuyViewModel
    {
        [Required]
        public BookListing listing { get; set; }
        [Required]
        public Book book { get; set; }
        [Required]
        public ApplicationUser seller { get; set; }
    }
}
