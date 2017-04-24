using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.TransactionViewModels
{
    public class SurveyViewModel
    {

        public string buyerId { get; set; }
        public int listingId { get; set; }
        public int logId { get; set; }
        public int finalPrice { get; set; }
        public int courseID { get; set; }
        public string condition { get; set; }

        public List<TransactionLog> openLogs { get; set; }
        public List<ApplicationUser> buyers { get; set; }
        public BookListing listing { get; set; }
        public List<Course> courses { get; set; }
    }
}
