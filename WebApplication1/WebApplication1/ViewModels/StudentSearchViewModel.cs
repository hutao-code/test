using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class StudentSearchViewModel
    {
        public string Keyword { get; set; }
        public string Grade { get; set; }
        public string Major { get; set; }
        public string SortBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<Student> Results { get; set; }
    }
}
