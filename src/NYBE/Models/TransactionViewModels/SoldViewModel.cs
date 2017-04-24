using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.TransactionViewModels
{
    public class SoldViewModel
    {
        public string buyerId { get; set; }
        public int listingId { get; set; }

        public List<TransactionLog> openLogs { get; set; }
        public List<ApplicationUser> buyers { get; set; }
    }
}
