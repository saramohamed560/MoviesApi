﻿using Movies.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BLL.Interfaces
{
    public interface IMovieRepository:IGenericRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetByCategoryIdAsync(int id);
    }
}
