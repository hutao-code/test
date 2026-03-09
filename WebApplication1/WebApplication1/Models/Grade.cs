using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [Required]
        [StringLength(8)]
        public string StudentId { get; set; }

        [Required]
        [StringLength(20)]
        public string CourseId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Score { get; set; }

        public DateTime RecordedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // 导航属性
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
