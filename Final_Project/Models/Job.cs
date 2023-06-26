using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string Jop_Title { get; set; }
        public string Jop_Description { get; set; }
        public string Jop_Requrement { get; set; }
        public DateTime DeadLine { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
