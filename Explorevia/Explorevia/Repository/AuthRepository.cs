using Explorevia.DTOs;
using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Claims;


namespace Explorevia.Repository
{
    public class AuthRepository : IAuthRepository
    {
        //DI
<<<<<<< HEAD
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _applicationUser;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthRepository(AppDbContext context, UserManager<ApplicationUser> applicationUser, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _applicationUser = applicationUser;
            _signInManager = signInManager;
        }

        public async Task<bool> Login(LoginViewModel ldto)
        {
           var user = await _applicationUser.FindByEmailAsync(ldto.Email);
            if(user != null)
            {
                var passwordCheck = await _applicationUser.CheckPasswordAsync(user, ldto.Password);
                if(passwordCheck)
                {
                    await _signInManager.SignInAsync(user, ldto.RememberMe);
                    return true;
                }
                return false;
            }
            return false;

        }

        

        public async Task<bool> RegisterUser(RegisterViewModel rdto)
        {
            var existingUser = await _applicationUser.FindByEmailAsync(rdto.Email);

            if (existingUser != null)
            {
                return false; // User with the same email already exists
            }
        

            var newUser = new ApplicationUser
            {
                UserName = rdto.Name,
                Email = rdto.Email,
                PasswordHash = rdto.Password,
                Role = "User"
            };
            var result =_applicationUser.CreateAsync(newUser, rdto.Password);
            if (result.IsCompletedSuccessfully)
            {
                await _signInManager.SignInAsync(newUser, false);
                return true;
=======
      // private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthRepository(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
        //   _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        

        public async Task<bool> RegisterUser(RegisterViewModel rdto)
        {
            var existingUser = await _userManager.FindByEmailAsync(rdto.Email);
            if (existingUser != null)
                return false;

            else
            {
                var newUser = new ApplicationUser
                {
                    UserName = rdto.Email,
                    Email = rdto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(rdto.Password),
                  
                };
                var result = await _userManager.CreateAsync(newUser, rdto.Password);//automatically save user to database amd save changes

                if (!result.Succeeded)
                    return false;

                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                await _userManager.AddToRoleAsync(newUser, "User");
                return true;
                //_context.Users.Add(newUser);
                //await _context.SaveChangesAsync();
            }

        }

        
        public async Task<bool> Login(LoginViewModel ldto)
            {
          
            ApplicationUser user = await _userManager.FindByEmailAsync(ldto.Email);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, ldto.Password);
                if(result == true)
                {
                   // List<Claim> claims = new List<Claim>();
                   // claims.Add(new Claim("","")) // if we want to keep info about user and save it 
                    await _signInManager.SignInAsync(user, ldto.RememberMe);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;


                //var result = await _signInManager.PasswordSignInAsync(
                //    ldto.Email,        // this must match ApplicationUser.UserName
                //    ldto.Password,
                //    isPersistent:ldto.RememberMe,
                //    lockoutOnFailure: true
                //);

                //return result.Succeeded;
>>>>>>> 5dcde25b1f1c760085716d479c40839990988c32
            }
            return false;

        }

<<<<<<< HEAD
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
=======

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
>>>>>>> 5dcde25b1f1c760085716d479c40839990988c32
        }

        /*  public async Task<bool> RegisterHotel(RegisterDTO rdto)
       {
           var existingUser = await _userManager.FindByEmailAsync(rdto.Email);

           if (existingUser != null)
           {
               return false; // User with the same email already exists
           }


           var newUser = new ApplicationUser
           {
               UserName = rdto.Email,
               Email = rdto.Email,
               PasswordHash = BCrypt.Net.BCrypt.HashPassword(rdto.Password)
           };
           var result = await _userManager.CreateAsync(newUser, rdto.Password);
           if (!result.Succeeded) return false;

           if (!await _roleManager.RoleExistsAsync("Hotel Owner"))
               await _roleManager.CreateAsync(new IdentityRole("Hotel Owner"));

           await _userManager.AddToRoleAsync(newUser, "Hotel Owner");


           //_context.Users.Add(newUser);
           //await _context.SaveChangesAsync();
           return true;
       }
     */


    }
}
