namespace BookManagementApi_GO.Model.ResponseModels
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public long TokenExpired { get; set; }
        public string RefreshToken { get; set; }
    }
}
