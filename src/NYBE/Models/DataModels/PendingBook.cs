using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class PendingBook
    {
        public int ID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string AuthorLName { get; set; }
        public string AuthorFName { get; set; }
        public string Description { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public string UserID { get; set; }
    }
}
