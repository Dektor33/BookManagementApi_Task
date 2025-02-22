using System.Text;
using System;
using BookManagementApi_GO.Model.Entities;
using System.Security.Cryptography;
using BookManagementApi_GO.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi_GO.DataAcess
{
    public interface IAuthRepository
    {
        Task<UserModel> GetUserByLogin ( string username , string password );
        Task RemoveRefreshTokenByUserID ( int userID );
        Task AddRefreshTokenModel ( RefreshTokenModel refreshTokenModel );
        Task<RefreshTokenModel> GetRefreshTokenTokenModel ( string refreshToken );

    }
    public class AuthRepository ( AppDbContext dbContext ) : IAuthRepository
    {

        public async Task AddRefreshTokenModel ( RefreshTokenModel refreshTokenModel )
        {
            await dbContext.RefreshTokens.AddAsync (refreshTokenModel);
            await dbContext.SaveChangesAsync ();
        }


        public Task<RefreshTokenModel> GetRefreshTokenTokenModel ( string refreshToken )
        {
            return dbContext.RefreshTokens.Include (n => n.User).ThenInclude (n => n.UserRoles).ThenInclude (n => n.Role).FirstOrDefaultAsync (n => n.RefreshToken == refreshToken);
        }


        public async Task<UserModel> GetUserByLogin ( string username , string password )
        {
            var user = await dbContext.Users
                .Include (n => n.UserRoles)
                .ThenInclude (n => n.Role)
                .FirstOrDefaultAsync (n => n.Username == username);

            if (user == null || !VerifyPassword (password , user.Password))
                return null; // Authentication failed

            return user;
        }




        public async Task RemoveRefreshTokenByUserID ( int userID )
        {
            var refreshToken = dbContext.RefreshTokens.FirstOrDefault (n => n.UserID == userID);
            if (refreshToken != null)
            {
                dbContext.RemoveRange (refreshToken);
                await dbContext.SaveChangesAsync ();
            }
        }


        //While adding and Validating Defaulr user data
        public async Task<UserModel?> AuthenticateAsync ( string username , string password )
        {
            var user = await dbContext.Users
                .Include (u => u.UserRoles)
                .ThenInclude (ur => ur.Role)
                .FirstOrDefaultAsync (u => u.Username == username);

            if (user == null || !VerifyPassword (password , user.Password))
                return null; // Authentication failed

            return user; // Return user if authentication is successful
        }


        private bool VerifyPassword ( string enteredPassword , string storedHash )
        {
            using (SHA256 sha256 = SHA256.Create ())
            {
                byte[] bytes = sha256.ComputeHash (Encoding.UTF8.GetBytes (enteredPassword));
                return Convert.ToBase64String (bytes) == storedHash;
            }
        }
    }
}
