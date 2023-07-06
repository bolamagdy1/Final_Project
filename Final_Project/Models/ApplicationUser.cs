using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name ="Full Name")]
        [MinLength(6, ErrorMessage = "6 is the minimum length")]
        [MaxLength(30, ErrorMessage = "30 is the maximum length")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{7,19}$", ErrorMessage = "User Name must start with alphabet and between 8 and 20 Characters")]
        public string FullName { get; set; }
    }
}
