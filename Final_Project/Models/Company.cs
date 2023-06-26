namespace Final_Project.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Logo { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Doc1 { get; set; }
        public string Doc2 { get; set; }


        public List<Job> Jobs { get; set; }

    }
}
