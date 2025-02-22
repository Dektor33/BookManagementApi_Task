using BookManagementApi_GO.DataAcess;
using BookManagementApi_GO.Model.Entities;

namespace BookManagementApi_GO.BusinessLogic
{
    public interface IAuthService
    {
        Task<UserModel> GetUserByLogin ( string username , string password );
        Task AddRefreshTokenModel ( RefreshTokenModel refreshTokenModel );
        Task<RefreshTokenModel> GetRefreshTokenTokenModel ( string refreshToken );
    }
    public class AuthService ( IAuthRepository authRepository ) : IAuthService
    {

        public async Task AddRefreshTokenModel ( RefreshTokenModel refreshTokenModel )
        {
            await authRepository.RemoveRefreshTokenByUserID (refreshTokenModel.UserID);
            await authRepository.AddRefreshTokenModel (refreshTokenModel);
        }


        public Task<RefreshTokenModel> GetRefreshTokenTokenModel ( string refreshToken )
        {
            return authRepository.GetRefreshTokenTokenModel (refreshToken);
        }


        public Task<UserModel> GetUserByLogin ( string username , string password )
        {
            return authRepository.GetUserByLogin (username , password);
        }
    }
}
