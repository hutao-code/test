using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class StudentManagementDbInitializer : CreateDatabaseIfNotExists<StudentManagementContext>
    {
        protected override void Seed(StudentManagementContext context)
        {
            // 添加种子课程数据
            var courses = new List<Course>
            {
                new Course
                {
                    CourseId = "CS101",
                    CourseName = "计算机科学导论",
                    Credits = 3,
                    Instructor = "张教授",
                    MaxStudents = 50,
                    Description = "计算机科学基础课程",
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    CourseId = "CS201",
                    CourseName = "数据结构与算法",
                    Credits = 4,
                    Instructor = "李教授",
                    MaxStudents = 40,
                    Description = "数据结构和算法设计",
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    CourseId = "CS301",
                    CourseName = "数据库系统",
                    Credits = 3,
                    Instructor = "王教授",
                    MaxStudents = 45,
                    Description = "关系数据库理论与实践",
                    CreatedAt = DateTime.Now
                },
                new Course
                {
                    CourseId = "CS401",
                    CourseName = "软件工程",
                    Credits = 3,
                    Instructor = "赵教授",
                    MaxStudents = 35,
                    Description = "软件开发方法论",
                    CreatedAt = DateTime.Now
                }
            };

            courses.ForEach(c => context.Courses.Add(c));
            context.SaveChanges();

            // 添加种子学生数据
            var students = new List<Student>
            {
                new Student
                {
                    StudentId = "20230001",
                    Name = "张三",
                    Age = 20,
                    Gender = "男",
                    Major = "计算机科学",
                    Grade = "2023级",
                    Email = "zhangsan@university.edu",
                    Phone = "13800138001",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    CreatedAt = DateTime.Now
                },
                new Student
                {
                    StudentId = "20230002",
                    Name = "李四",
                    Age = 19,
                    Gender = "女",
                    Major = "软件工程",
                    Grade = "2023级",
                    Email = "lisi@university.edu",
                    Phone = "13800138002",
                    EnrollmentDate = new DateTime(2023, 9, 1),
                    CreatedAt = DateTime.Now
                },
                new Student
                {
                    StudentId = "20220001",
                    Name = "王五",
                    Age = 21,
                    Gender = "男",
                    Major = "数据科学",
                    Grade = "2022级",
                    Email = "wangwu@university.edu",
                    Phone = "13800138003",
                    EnrollmentDate = new DateTime(2022, 9, 1),
                    CreatedAt = DateTime.Now
                },
                new Student
                {
                    StudentId = "20220002",
                    Name = "赵六",
                    Age = 22,
                    Gender = "女",
                    Major = "信息安全",
                    Grade = "2022级",
                    Email = "zhaoliu@university.edu",
                    Phone = "13800138004",
                    EnrollmentDate = new DateTime(2022, 9, 1),
                    CreatedAt = DateTime.Now
                },
                new Student
                {
                    StudentId = "20210001",
                    Name = "孙七",
                    Age = 23,
                    Gender = "男",
                    Major = "计算机科学",
                    Grade = "2021级",
                    Email = "sunqi@university.edu",
                    Phone = "13800138005",
                    EnrollmentDate = new DateTime(2021, 9, 1),
                    CreatedAt = DateTime.Now
                }
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            // 添加种子课程注册数据
            var registrations = new List<CourseRegistration>
            {
                new CourseRegistration
                {
                    StudentId = "20230001",
                    CourseId = "CS101",
                    RegisteredAt = DateTime.Now.AddDays(-30)
                },
                new CourseRegistration
                {
                    StudentId = "20230001",
                    CourseId = "CS201",
                    RegisteredAt = DateTime.Now.AddDays(-30)
                },
                new CourseRegistration
                {
                    StudentId = "20230002",
                    CourseId = "CS101",
                    RegisteredAt = DateTime.Now.AddDays(-29)
                },
                new CourseRegistration
                {
                    StudentId = "20220001",
                    CourseId = "CS301",
                    RegisteredAt = DateTime.Now.AddDays(-60)
                },
                new CourseRegistration
                {
                    StudentId = "20220002",
                    CourseId = "CS301",
                    RegisteredAt = DateTime.Now.AddDays(-60)
                }
            };

            registrations.ForEach(r => context.CourseRegistrations.Add(r));
            context.SaveChanges();

            // 添加种子成绩数据
            var grades = new List<Grade>
            {
                new Grade
                {
                    StudentId = "20230001",
                    CourseId = "CS101",
                    Score = 85.5m,
                    RecordedAt = DateTime.Now.AddDays(-10)
                },
                new Grade
                {
                    StudentId = "20230002",
                    CourseId = "CS101",
                    Score = 92.0m,
                    RecordedAt = DateTime.Now.AddDays(-10)
                },
                new Grade
                {
                    StudentId = "20220001",
                    CourseId = "CS301",
                    Score = 78.5m,
                    RecordedAt = DateTime.Now.AddDays(-5)
                },
                new Grade
                {
                    StudentId = "20220002",
                    CourseId = "CS301",
                    Score = 88.0m,
                    RecordedAt = DateTime.Now.AddDays(-5)
                }
            };

            grades.ForEach(g => context.Grades.Add(g));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
