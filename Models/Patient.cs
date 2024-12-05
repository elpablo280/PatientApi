using System.ComponentModel.DataAnnotations;

namespace PatientApi.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public Name Name { get; set; }
        public string Gender { get; set; }
        //[Required]
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
    }

    public class Name
    {
        public Guid Id { get; set; }
        public string Use { get; set; }
        //[Required]
        public string Family { get; set; }
        public List<string> Given { get; set; }
    }
}
