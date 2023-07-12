using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Accepted
    {
        public int AcceptedId { get; set; }
        public DateTime Date { get; set; }
        public string Extra { get; set; }

        //Relations
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public Job Job { get; set; }
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
    }
}
