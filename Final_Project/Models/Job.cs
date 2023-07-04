using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Job
    {
        public int JobId { get; set; }
        [Display(Name ="Job Title")]
        [MinLength(6)]
        public string Jop_Title { get; set; }
        [Display(Name = "Job Description")]
        [MinLength (6)]
        public string Jop_Description { get; set; }
        [Display(Name ="Job Requrements")]
        [Required]
        public string Jop_Requrement { get; set; }
        [Display(Name = "Dead Line")]
        [Required]
        public DateTime DeadLine { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
