using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBluAppFinal.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
    }
}
