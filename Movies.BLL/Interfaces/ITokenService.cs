using Microsoft.AspNetCore.Identity;
using Movies.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BLL.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user ,UserManager<AppUser> userManager);
    }
}
