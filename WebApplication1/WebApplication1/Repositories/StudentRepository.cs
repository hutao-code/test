using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 学生数据访问实现类，提供学生数据的持久化操作
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentManagementContext _context;

        public StudentRepository(StudentManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region 基本 CRUD 操作

        /// <summary>
        /// 获取所有学生
        /// </summary>
        public IEnumerable<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        /// <summary>
        /// 根据学号获取学生
        /// </summary>
        public Student GetById(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                return null;

            return _context.Students.Find(studentId);
        }

        /// <summary>
        /// 添加新学生
        /// </summary>
        public void Add(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            using (var transaction = new TransactionScope())
            {
                student.CreatedAt = DateTime.Now;
                _context.Students.Add(student);
                _context.SaveChanges();
                transaction.Complete();
            }
        }

        /// <summary>
        /// 更新学生信息
        /// </summary>
        public void Update(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            using (var transaction = new TransactionScope())
            {
                var existing = _context.Students.Find(student.StudentId);
                if (existing != null)
                {
                    existing.Name = student.Name;
                    existing.Age = student.Age;
                    existing.Gender = student.Gender;
                    existing.Major = student.Major;
                    existing.Grade = student.Grade;
                    existing.Email = student.Email;
                    existing.Phone = student.Phone;
                    existing.EnrollmentDate = student.EnrollmentDate;
                    existing.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                    transaction.Complete();
                }
            }
        }

        /// <summary>
        /// 删除学生
        /// </summary>
        public void Delete(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                return;

            using (var transaction = new TransactionScope())
            {
                var student = _context.Students.Find(studentId);
                if (student != null)
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                    transaction.Complete();
                }
            }
        }

        #endregion

        #region 查询操作

        /// <summary>
        /// 按关键字搜索学生（姓名）
        /// </summary>
        public IEnumerable<Student> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Student>();

            return _context.Students
                .Where(s => s.Name.Contains(keyword))
                .ToList();
        }

        /// <summary>
        /// 按年级获取学生
        /// </summary>
        public IEnumerable<Student> GetByGrade(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return new List<Student>();

            return _context.Students
                .Where(s => s.Grade == grade)
                .ToList();
        }

        /// <summary>
        /// 按专业获取学生
        /// </summary>
        public IEnumerable<Student> GetByMajor(string major)
        {
            if (string.IsNullOrWhiteSpace(major))
                return new List<Student>();

            return _context.Students
                .Where(s => s.Major == major)
                .ToList();
        }

        /// <summary>
        /// 分页获取学生列表
        /// </summary>
        public IEnumerable<Student> GetPaged(int page, int pageSize, string sortBy)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var query = _context.Students.AsQueryable();

            // 排序
            switch (sortBy?.ToLower())
            {
                case "studentid":
                    query = query.OrderBy(s => s.StudentId);
                    break;
                case "enrollmentdate":
                    query = query.OrderBy(s => s.EnrollmentDate);
                    break;
                case "name":
                default:
                    query = query.OrderBy(s => s.Name);
                    break;
            }

            // 分页
            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        #endregion

        #region 统计操作

        /// <summary>
        /// 获取学生总数
        /// </summary>
        public int GetTotalCount()
        {
            return _context.Students.Count();
        }

        /// <summary>
        /// 按年级统计学生数量
        /// </summary>
        public Dictionary<string, int> GetCountByGrade()
        {
            return _context.Students
                .GroupBy(s => s.Grade)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// 按专业统计学生数量
        /// </summary>
        public Dictionary<string, int> GetCountByMajor()
        {
            return _context.Students
                .GroupBy(s => s.Major)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        #endregion

        #region 关联操作

        /// <summary>
        /// 获取学生的所有课程
        /// </summary>
        public IEnumerable<Course> GetStudentCourses(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                return new List<Course>();

            return _context.CourseRegistrations
                .Where(cr => cr.StudentId == studentId)
                .Include(cr => cr.Course)
                .Select(cr => cr.Course)
                .ToList();
        }

        /// <summary>
        /// 获取学生的所有成绩
        /// </summary>
        public IEnumerable<Grade> GetStudentGrades(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                return new List<Grade>();

            return _context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.Course)
                .ToList();
        }

        #endregion

        #region 事务支持

        /// <summary>
        /// 删除学生及其关联数据（课程注册和成绩记录）
        /// </summary>
        public void DeleteWithRelatedData(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                return;

            using (var transaction = new TransactionScope())
            {
                var student = _context.Students.Find(studentId);
                if (student != null)
                {
                    // 由于配置了级联删除，直接删除学生即可
                    // Entity Framework 会自动删除关联的 CourseRegistrations 和 Grades
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                    transaction.Complete();
                }
            }
        }

        #endregion
    }
}
