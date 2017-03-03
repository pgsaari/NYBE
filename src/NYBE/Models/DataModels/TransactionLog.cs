using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class TransactionLog
    {
        public int ID { get; set; }
        public string SellerID { get; set; }
        public string BuyerID { get; set; }
        public int BookID { get; set; }
        public double TransRating { get; set; }
        public int Status { get; set; } // 1 for active, 0 for closed?
        public DateTime TransDate { get; set; }

        public ApplicationUser Seller { get; set; }
        public ApplicationUser Buyer { get; set; }
        public Book Book { get; set; }

    }
}
