using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eBluAppFinal.Models;

namespace eBluAppFinal.Data
{
    public class eBluAppContext : DbContext
    {
        public eBluAppContext (DbContextOptions<eBluAppContext> options)
            : base(options)
        {
        }

        public DbSet<eBluAppFinal.Models.Book> Book { get; set; }

        public DbSet<eBluAppFinal.Models.User> User { get; set; }
    }
}
