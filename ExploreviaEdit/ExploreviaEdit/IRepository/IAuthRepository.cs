using ExploreviaEdit.ViewModel;

namespace ExploreviaEdit.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUser(RegisterViewModel rdto);
        Task<bool> Login(LoginViewModel ldto);
        Task<bool> LogoutAsync();
    }
}
