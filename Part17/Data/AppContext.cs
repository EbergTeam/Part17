using Microsoft.EntityFrameworkCore;
using Part17.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Part17.Data
{
    public class MyAppContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
        {
            //
        }
    }
}
