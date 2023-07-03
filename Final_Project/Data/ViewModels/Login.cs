using System.ComponentModel.DataAnnotations;

namespace Final_Project.Data.ViewModels
{
    public class Login
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
