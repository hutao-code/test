using System.Collections.Generic;

namespace WebApplication1.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalStudents { get; set; }
        public Dictionary<string, int> StudentsByGrade { get; set; }
        public Dictionary<string, int> StudentsByMajor { get; set; }
        public Dictionary<string, int> CourseRegistrationCounts { get; set; }
    }
}
