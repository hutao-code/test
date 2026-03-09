using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CourseRegistration
    {
        public int RegistrationId { get; set; }

        [Required]
        [StringLength(8)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(20)]
        public string CourseId { get; set; }

        public DateTime RegisteredAt { get; set; }

        // 导航属性
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
