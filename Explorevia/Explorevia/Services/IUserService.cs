//using Explorevia.Data;
//using Explorevia.Models;
////using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Explorevia.Services
//{
//    public interface IUserService
//    {
//        Task<User> RegisterAsync(string name, string email, string password, string role = "User");
//        Task<User> LoginAsync(string email, string password);
//        Task<IEnumerable<User>> GetAllUsersAsync();
//    }

//    public class UserService : IUserService
//    {
//        private readonly AppDbContext _context;
//        //private readonly IPasswordHasher _hasher;

//        public UserService(AppDbContext context)
//        {
//            _context = context;
            
//        }

//        public async Task<User> RegisterAsync(string name, string email, string password, string role = "User")
//        {
//            if (await _context.Users.AnyAsync(u => u.Email == email)) return null;
//            var user = new User
//            {
//                Name = name,
//                Email = email,
//                //PasswordHash = _hasher.Hash(password),
//                Role = role
//            };
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//            return user;
//        }

//        public async Task<User> LoginAsync(string email, string password)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
//            //if (user == null || !_hasher.Verify(password, user.PasswordHash)) return null;
//            return user;
//        }

//        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();
//    }
//}


