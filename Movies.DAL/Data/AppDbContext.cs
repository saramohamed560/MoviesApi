using Microsoft.EntityFrameworkCore;
using Movies.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DAL.Data
{
    public  class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
                
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
