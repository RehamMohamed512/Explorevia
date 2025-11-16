using Explorevia.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Explorevia.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUser(RegisterDTO rdto);
        Task<bool> Login(LoginDTO ldto);
        Task<bool> RegisterHotel(RegisterDTO rdto);
    }
}
