using Explorevia.DTOs;
using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Login(LoginDTO ldto)
        {
           var user = _context.Users.FirstOrDefault(u=>u.Email == ldto.Email);
            if (user == null) 
            {
                return false;
            }

            bool isPasswordVaild = BCrypt.Net.BCrypt.Verify(ldto.Password,user.PasswordHash);
            if (!isPasswordVaild) 
                return false;
            return true;
           
        }

        public async Task<bool> RegisterUser(RegisterDTO rdto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == rdto.Email);

            if (existingUser != null)
            {
                return false; // User with the same email already exists
            }
        

            var newUser = new User
            {
                Name = rdto.Name,
                Email = rdto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(rdto.Password),
                Role = "User"
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> RegisterHotel(RegisterDTO rdto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == rdto.Email);

            if (existingUser != null)
            {
                return false; // User with the same email already exists
            }


            var newUser = new User
            {
                Name = rdto.Name,
                Email = rdto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(rdto.Password),
                Role = "Hotel Owner"
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
