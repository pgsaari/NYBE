using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.BookViewModels
{
    public class PendingBookViewModel
    {
        public int id { get; set; }

        [Required]
        public string isbn { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string authorLName { get; set; }

        [Required]
        public string authorFName { get; set; }

        public string description { get; set; }
        public string edition { get; set; }
        public string publisher { get; set; }
    }
}
