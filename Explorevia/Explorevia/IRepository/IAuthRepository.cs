using Explorevia.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Explorevia.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUser(RegisterViewModel rdto);
        Task<bool> Login(LoginViewModel ldto);
        Task<bool> LogoutAsync();
    }
}
