using Explorevia.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Explorevia.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUser(RegisterViewModel rdto);
        Task<bool> Login(LoginViewModel ldto);
<<<<<<< HEAD
        Task<bool> Logout();
=======
        Task LogoutAsync();
        // Task<bool> RegisterHotel(RegisterDTO rdto);
>>>>>>> 5dcde25b1f1c760085716d479c40839990988c32
    }
}
