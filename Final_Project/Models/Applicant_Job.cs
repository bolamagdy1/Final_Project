using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Applicant_Job
    {
        public int Id { get; set; }
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
