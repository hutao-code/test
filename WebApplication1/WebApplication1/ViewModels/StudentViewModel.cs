using System;

namespace WebApplication1.ViewModels
{
    public class StudentViewModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
