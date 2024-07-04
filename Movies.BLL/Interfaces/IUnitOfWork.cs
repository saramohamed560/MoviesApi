using Movies.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BLL.Interfaces
{
    public  interface IUnitOfWork :IAsyncDisposable
    {
        public ICategoryRepository CategoryRepository { get; set; }
        public IMovieRepository MovieRepository { get; set; }

        Task<int> Complete();
    }
}
