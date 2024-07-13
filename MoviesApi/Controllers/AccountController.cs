using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Movies.BLL.Interfaces;
using Movies.DAL.Entities;
using Movies.PL.DTOs;
using System.Security.Claims;

namespace Movies.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<AppUserDto>>Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest("This Email is already exist");
            var user = new AppUser()
            {
                Email=model.Email,
                UserName=model.UserName,
                PhoneNumber=model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest();
            var returnedUser = new AppUserDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user,_userManager)
            };
            return Ok(returnedUser);



        }
        [HttpPost("Login")]
        public async Task<ActionResult<AppUserDto>> login(LoginDto dto) {
         var user=await _userManager.FindByEmailAsync(dto.Email);
         if (user is null) return Unauthorized("Email Doesnot Exists");
         var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
         if (!result.Succeeded) return Unauthorized("You are not allowed");
         var returnedUser = new AppUserDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user,_userManager)
         };
          return Ok(returnedUser);

        
        }

        [HttpGet("GetCurretUser")]
        public async Task<ActionResult<AppUserDto>> GetCurretUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var returnedUser = new AppUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            };
            return Ok(returnedUser);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            //var user = _userManager.FindByEmailAsync(email);
            //if (user is null) return false;
            //return true;
            return await _userManager.FindByEmailAsync(email) is not null;

        }
    }
}
