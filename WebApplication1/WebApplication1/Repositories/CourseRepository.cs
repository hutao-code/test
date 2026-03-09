using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 课程数据访问实现类
    /// </summary>
    public class CourseRepository : ICourseRepository
    {
        private readonly StudentManagementContext _context;

        public CourseRepository(StudentManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetById(string courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public void Add(Course course)
        {
            course.CreatedAt = DateTime.Now;
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void Update(Course course)
        {
            var existing = _context.Courses.Find(course.CourseId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(course);
                _context.SaveChanges();
            }
        }

        public void Delete(string courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }
    }
}
