using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public class School
    {
        public int ID { get;  set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Required]
        public int Status { get; set; } // 0 for inactive; 1 for active

    }
}
