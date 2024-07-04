using Movies.BLL.Interfaces;
using Movies.DAL.Data;
using Movies.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BLL.Repositories
{
    public  class MovieRepository :GenericRepository<Movie>,IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context):base(context)
        {
            _context = context;
        }
    }
}
