using Movies.BLL.Interfaces;
using Movies.BLL.Repositories;
using Movies.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository CategoryRepository { get ; set ; }
        public IMovieRepository MovieRepository { get ; set ; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(context);
            MovieRepository = new MovieRepository(context);
        }

        public async ValueTask DisposeAsync()
        {
             await _context.DisposeAsync();
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
