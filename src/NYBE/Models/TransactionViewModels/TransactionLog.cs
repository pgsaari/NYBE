using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class TransactionLog
    {
        public TransactionLog() { }

        public TransactionLog(string sellerID, string buyerID, int book, double rating, int status, double price, string condition, DateTime date)
        {
            SellerID = sellerID;
            BuyerID = buyerID;
            BookID = book;
            TransRating = rating;
            Status = status;
            TransDate = date;
            SoldPrice = price;
            Condition = condition;
        }

        public int ID { get; set; }
        public string SellerID { get; set; }
        public string BuyerID { get; set; }
        public int BookID { get; set; }
        public double TransRating { get; set; }
        public int Status { get; set; } // 1 for open, 0 for closed

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public double SoldPrice { get; set; } // how did i forget this...? -ps
        public string Condition { get; set; }
        public string Comments { get; set; }
        public DateTime TransDate { get; set; }

        public ApplicationUser Seller { get; set; }
        public ApplicationUser Buyer { get; set; }
        public Book Book { get; set; }

    }
}
