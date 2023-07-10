using Final_Project.Data.Category;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Display(Name ="Course Name")]
        [MinLength(3)]
        public string CourseName { get; set; }
        [Display(Name ="Image")]
        public string Image { get; set; }
        [Display(Name = "Course Description")]
        [MinLength (6)]
        public string CourseDescription { get; set; }
        [Display(Name = "Course Content")]
        [MinLength(6)]
        public string Content { get; set; }
        [Display(Name ="Course Price")]
        public double Price { get; set; }
        [Display(Name ="Category")]
        public Category Category { get; set; }

        //Relation
        public List<Course_Videos> C_Vs { get; set; }
    }
}
