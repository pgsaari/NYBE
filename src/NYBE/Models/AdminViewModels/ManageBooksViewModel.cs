using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYBE.Models.AdminViewModels
{
    public class ManageBooksViewModel
    {
        public List<PendingBook> pendingBooks { get; set; }
        public List<Book> allBooks { get; set; }
        public List<Book> disabledBooks { get; set; }
    }
}
