using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Applicant
    {

        public int ApplicantId { get; set; }
        [Display(Name ="Full Name")]
        [MinLength(6,ErrorMessage = "6 is the minimum length")]
        [MaxLength(30, ErrorMessage = "30 is the maximum length")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{7,19}$",ErrorMessage = "User Name must start with alphabet and between 8 and 20 Characters")]
        public string ApplicantName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage ="Invalid Email Adderss")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Profile Picture")]
        public string? Picture { get; set; }
        [Display(Name ="Applicant Tilte")]
        [MinLength(6, ErrorMessage = "6 is the minimum length")]
        public string Title { get; set; }
        [MinLength(6, ErrorMessage = "6 is the minimum length")]
        public string Address { get; set; }
        [Display(Name ="Phone Number")]
        [RegularExpression("^01[0-2,5]{1}[0-9]{8}$",ErrorMessage ="Phone number is not Correct")]
        [StringLength(11)]
        public string Phone { get; set; }
        [Display(Name ="Age")]
        [Range(15,60,ErrorMessage ="Age must be from 15 to 60")]
        public int Age { get; set; }
        [Display(Name = "CV")]
        public string? CV { get; set; }

    }
}
