using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Course
    {
        [Required]
        [StringLength(20)]
        public string CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        [StringLength(50)]
        public string Instructor { get; set; }

        [Required]
        public int MaxStudents { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        // 导航属性
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
