using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Applicant_Course
    {
        public int Id { get; set; }
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
