using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Applicant
    {
        public int ApplicantId { get; set; }
        [Display(Name ="Full Name")]
        [MinLength(6,ErrorMessage = "6 is the minimum length")]
        [MaxLength(30, ErrorMessage = "30 is the maximum length")]
        public string ApplicantName { get; set; }
        [Display(Name ="Profile Picture")]
        public byte[] Picture { get; set; }
        [Display(Name ="Applicant Tilte")]
        [MinLength(6, ErrorMessage = "6 is the minimum length")]
        public string Title { get; set; }
        [MinLength(6, ErrorMessage = "6 is the minimum length")]
        public string Address { get; set; }
        [Display(Name ="Phone Number")]
        [MaxLength(13)]
        [Phone]
        public string Phone { get; set; }
        [Display(Name ="Age")]
        public int Age { get; set; }
        [Display(Name = "CV")]
        public byte[] CV { get; set; }

    }
}
