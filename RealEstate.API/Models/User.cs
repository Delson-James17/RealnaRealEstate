namespace RealEstate.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string Address { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string UrlImages { get; set; }
        public string? Zoomlink { get; set; }
        public List<Appointment>Appointments { get; set; }

        public User()
        {
            
        }
        public User(int id, string name, string email, string address, int age, DateTime dOB)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Age = age;
            DOB = dOB;
            
        }
    }
}
