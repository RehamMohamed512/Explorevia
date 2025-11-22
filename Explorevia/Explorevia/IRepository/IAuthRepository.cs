using Explorevia.DTOs;
using Explorevia.Models;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Explorevia.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUser(RegisterViewModel rdto);
        Task<bool> RegisterHotelOwner(HotelOwnerRegisterViewModel hvm);
        Task<string> Login(LoginViewModel ldto);
        Task<bool> LogoutAsync();
    }
}
