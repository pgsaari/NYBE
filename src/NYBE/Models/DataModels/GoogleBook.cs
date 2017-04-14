using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.DataModels
{
    public class GoogleBook
    {
        public string title { get; set; }
        public string authorFName { get; set; }
        public string authorLName { get; set; }
        public string isbn { get; set; }
        public string description { get; set; }
        public string image { get; set; }

    }
}
