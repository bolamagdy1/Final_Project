using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Course_Videos
    {
        public int Id { get; set; }
        [Display(Name ="Video")]
        public string Videos { get; set; }

        //Relation
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
