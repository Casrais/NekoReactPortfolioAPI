using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CosmosAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Identity> userManager;
        private readonly SignInManager<Identity> signInManager;
        private readonly TokenService tokenService;

        public AccountController(UserManager<Identity> userManager, SignInManager<Identity> signInManager, TokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, string ReturnUrl)
        {

            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded) 
            {
                if(!string.IsNullOrEmpty(ReturnUrl)) { Redirect(ReturnUrl); }
                else { RedirectToAction("index", "Home");}
                return CreateUserObject(user);
            }
            RedirectToAction("index", "home");
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //if (await userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            if (await userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email taken");
            }
            if (await userManager.FindByNameAsync(registerDto.Username) != null)
            {
                return BadRequest("Username taken");
            }

            var user = new Identity
            {
                //DisplayName = registerDto.DisplayName,
                UserName = registerDto.Username,
                Email = registerDto.Email
            };


            var result = await userManager.CreateAsync(user, registerDto.Password);

            

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem registering user.");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }


        private UserDto CreateUserObject(Identity user)
        {
            return new UserDto
            {
                Token = tokenService.CreateToken(user),
                Username = user.UserName,
                //DisplayName = user.DisplayName
            };
        }


        //[Authorize]
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        //}



    }
}
