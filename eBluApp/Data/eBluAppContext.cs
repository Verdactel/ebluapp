using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eBluApp.Models;

namespace eBluApp.Data
{
    public class eBluAppContext : DbContext
    {
        public eBluAppContext (DbContextOptions<eBluAppContext> options)
            : base(options)
        {
        }

        public DbSet<eBluApp.Models.Book> Book { get; set; }

        public DbSet<eBluApp.Models.Login> Login { get; set; }
    }
}
