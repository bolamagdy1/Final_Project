namespace Final_Project.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Image { get; set; }
        public string CourseDescription { get; set; }
        public string Content { get; set; }
        public double Price { get; set; }

        //Relation
        public List<Course_Videos> C_Vs { get; set; }
    }
}
