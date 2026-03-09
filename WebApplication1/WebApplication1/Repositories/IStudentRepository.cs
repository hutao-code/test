using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 学生数据访问接口，定义学生数据的持久化操作
    /// </summary>
    public interface IStudentRepository
    {
        // 基本 CRUD 操作
        
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns>学生集合</returns>
        IEnumerable<Student> GetAll();

        /// <summary>
        /// 根据学号获取学生
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>学生对象，如果不存在则返回 null</returns>
        Student GetById(string studentId);

        /// <summary>
        /// 添加新学生
        /// </summary>
        /// <param name="student">学生对象</param>
        void Add(Student student);

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student">学生对象</param>
        void Update(Student student);

        /// <summary>
        /// 删除学生
        /// </summary>
        /// <param name="studentId">学号</param>
        void Delete(string studentId);

        // 查询操作

        /// <summary>
        /// 按关键字搜索学生（姓名）
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <returns>匹配的学生集合</returns>
        IEnumerable<Student> Search(string keyword);

        /// <summary>
        /// 按年级获取学生
        /// </summary>
        /// <param name="grade">年级</param>
        /// <returns>该年级的学生集合</returns>
        IEnumerable<Student> GetByGrade(string grade);

        /// <summary>
        /// 按专业获取学生
        /// </summary>
        /// <param name="major">专业</param>
        /// <returns>该专业的学生集合</returns>
        IEnumerable<Student> GetByMajor(string major);

        /// <summary>
        /// 分页获取学生列表
        /// </summary>
        /// <param name="page">页码（从1开始）</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortBy">排序字段（Name, StudentId, EnrollmentDate）</param>
        /// <returns>分页后的学生集合</returns>
        IEnumerable<Student> GetPaged(int page, int pageSize, string sortBy);

        // 统计操作

        /// <summary>
        /// 获取学生总数
        /// </summary>
        /// <returns>学生总数</returns>
        int GetTotalCount();

        /// <summary>
        /// 按年级统计学生数量
        /// </summary>
        /// <returns>年级-学生数量字典</returns>
        Dictionary<string, int> GetCountByGrade();

        /// <summary>
        /// 按专业统计学生数量
        /// </summary>
        /// <returns>专业-学生数量字典</returns>
        Dictionary<string, int> GetCountByMajor();

        // 关联操作

        /// <summary>
        /// 获取学生的所有课程
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>课程集合</returns>
        IEnumerable<Course> GetStudentCourses(string studentId);

        /// <summary>
        /// 获取学生的所有成绩
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>成绩集合</returns>
        IEnumerable<Grade> GetStudentGrades(string studentId);

        // 事务支持

        /// <summary>
        /// 删除学生及其关联数据（课程注册和成绩记录）
        /// </summary>
        /// <param name="studentId">学号</param>
        void DeleteWithRelatedData(string studentId);
    }
}
