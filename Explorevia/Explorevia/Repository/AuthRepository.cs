using Explorevia.DTOs;
using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;


namespace Explorevia.Repository
{
    public class AuthRepository : IAuthRepository
    {
        //DI
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
            }
            return false;

        }

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }



    }
}
