using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 课程注册数据访问实现类
    /// </summary>
    public class CourseRegistrationRepository : ICourseRegistrationRepository
    {
        private readonly StudentManagementContext _context;
        private readonly ICourseRepository _courseRepository;

        public CourseRegistrationRepository(StudentManagementContext context, ICourseRepository courseRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
        }

        public void Register(string studentId, string courseId)
        {
            // 检查是否已注册
            if (IsRegistered(studentId, courseId))
            {
                throw new InvalidOperationException($"学生 {studentId} 已注册课程 {courseId}");
            }

            // 检查课程容量
            var course = _courseRepository.GetById(courseId);
            if (course == null)
            {
                throw new InvalidOperationException($"课程 {courseId} 不存在");
            }

            var registrationCount = GetCourseRegistrationCount(courseId);
            if (registrationCount >= course.MaxStudents)
            {
                throw new InvalidOperationException($"课程 {courseId} 已达到最大注册人数");
            }

            // 使用事务确保原子性
            using (var transaction = new TransactionScope())
            {
                var registration = new CourseRegistration
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    RegisteredAt = DateTime.Now
                };

                _context.CourseRegistrations.Add(registration);
                _context.SaveChanges();
                transaction.Complete();
            }
        }

        public void Unregister(string studentId, string courseId)
        {
            var registration = _context.CourseRegistrations
                .FirstOrDefault(r => r.StudentId == studentId && r.CourseId == courseId);

            if (registration != null)
            {
                _context.CourseRegistrations.Remove(registration);
                _context.SaveChanges();
            }
        }

        public IEnumerable<CourseRegistration> GetByStudent(string studentId)
        {
            return _context.CourseRegistrations
                .Include(r => r.Course)
                .Where(r => r.StudentId == studentId)
                .ToList();
        }

        public IEnumerable<CourseRegistration> GetByCourse(string courseId)
        {
            return _context.CourseRegistrations
                .Include(r => r.Student)
                .Where(r => r.CourseId == courseId)
                .ToList();
        }

        public bool IsRegistered(string studentId, string courseId)
        {
            return _context.CourseRegistrations
                .Any(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public int GetCourseRegistrationCount(string courseId)
        {
            return _context.CourseRegistrations
                .Count(r => r.CourseId == courseId);
        }
    }
}
