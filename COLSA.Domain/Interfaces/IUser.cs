using COLSA.Domain.Models;

namespace COLSA.Domain.Interfaces
{
    public interface IUser : IGeneralAsyncRepo<UserModel>
    {
        Task<string> UserRegister(UserModel user, string password);
        Task<Object> UserLogin(string? userName, string? password);
        Task<bool> UserExist(string userName);
    }
}