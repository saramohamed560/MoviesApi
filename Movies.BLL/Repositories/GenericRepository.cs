using Microsoft.EntityFrameworkCore;
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
    public class GenericRepository<T> :IGenericRepository<T> where T :BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public  async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Movie))
                return (IEnumerable<T>) await _context.Movies.Include(M=>M.Category).ToListAsync();
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int? id)
        {
            if (typeof(T) == typeof(Movie))
                return  await _context.Movies.Include(m=>m.Category).FirstOrDefaultAsync(m=>m.Id==id) as T;
            return await _context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
