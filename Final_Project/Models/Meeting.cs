using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Agenda { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public int Duration { get; set; }
    }
}
