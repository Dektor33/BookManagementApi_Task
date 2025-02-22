using BookManagementApi_GO.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BookManagementApi_GO.Database.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext ( DbContextOptions<AppDbContext> options ) : base (options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public DbSet<UserRoleModel> UserRoles { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            base.OnModelCreating (modelBuilder);

            // Seed roles
            var adminRole = new RoleModel { Id = 1 , RoleName = "Admin" };
            var readerRole = new RoleModel { Id = 2 , RoleName = "Reader" };

            // Hash passwords
            string adminPassword = HashPassword ("Admin123");
            string readerPassword = HashPassword ("Reader123");

            // Seed users
            var adminUser = new UserModel { Id = 1 , Username = "admin" , Password = adminPassword };
            var readerUser = new UserModel { Id = 2 , Username = "reader" , Password = readerPassword };

            // Seed user roles
            var adminUserRole = new UserRoleModel { ID = 1 , UserID = 1 , RoleID = 1 };
            var readerUserRole = new UserRoleModel { ID = 2 , UserID = 2 , RoleID = 2 };

            // Add data to the model
            modelBuilder.Entity<RoleModel> ().HasData (adminRole , readerRole);
            modelBuilder.Entity<UserModel> ().HasData (adminUser , readerUser);
            modelBuilder.Entity<UserRoleModel> ().HasData (adminUserRole , readerUserRole);
        }

        private static string HashPassword ( string password )
        {
            using var sha256 = SHA256.Create ();
            byte[] bytes = sha256.ComputeHash (Encoding.UTF8.GetBytes (password));
            return Convert.ToBase64String (bytes);
        }
    }
}
