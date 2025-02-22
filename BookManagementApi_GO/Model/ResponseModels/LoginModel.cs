using System.ComponentModel.DataAnnotations;

namespace BookManagementApi_GO.Model.ResponseModels
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
