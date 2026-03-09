using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 成绩数据访问接口
    /// </summary>
    public interface IGradeRepository
    {
        /// <summary>
        /// 添加或更新成绩记录
        /// </summary>
        /// <param name="grade">成绩对象</param>
        void AddOrUpdate(Grade grade);

        /// <summary>
        /// 获取学生的所有成绩
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>成绩集合</returns>
        IEnumerable<Grade> GetByStudent(string studentId);

        /// <summary>
        /// 获取课程的所有成绩
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <returns>成绩集合</returns>
        IEnumerable<Grade> GetByCourse(string courseId);

        /// <summary>
        /// 获取学生在特定课程的成绩
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="courseId">课程ID</param>
        /// <returns>成绩对象，如果不存在则返回 null</returns>
        Grade GetByStudentAndCourse(string studentId, string courseId);

        /// <summary>
        /// 计算学生的平均成绩
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>平均成绩</returns>
        decimal CalculateAverageGrade(string studentId);

        /// <summary>
        /// 获取课程的成绩分布
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <returns>成绩区间和学生数量的字典</returns>
        Dictionary<string, int> GetGradeDistribution(string courseId);
    }
}
