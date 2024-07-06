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
    public  class CategoryRepository :GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task< bool> IsValidCategory(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
