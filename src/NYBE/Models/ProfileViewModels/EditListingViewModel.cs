using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.DataModels
{
    public class EditListingViewModel
    {
        public int ID { get; set; }
        public string condition { get; set; }
        public double price { get; set; }
        public Book book { get; set; }
        public Course course { get; set; }
    }
}
