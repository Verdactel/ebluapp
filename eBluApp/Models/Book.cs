using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBluApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
