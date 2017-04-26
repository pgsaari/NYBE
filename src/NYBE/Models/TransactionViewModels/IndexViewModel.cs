using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.TransactionViewModels
{
    public class IndexViewModel
    {
        public double TransRating { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double SoldPrice { get; set; }
        public string Condition { get; set; }
        public string Comments { get; set; }
        public Book Book;
        public ApplicationUser Seller;
        public ApplicationUser Buyer;
    }
}
