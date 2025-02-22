using System.ComponentModel.DataAnnotations;

namespace BookManagementApi_GO.Model.Entities
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Store hashed password

        public virtual ICollection<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel> ();
    }
}
