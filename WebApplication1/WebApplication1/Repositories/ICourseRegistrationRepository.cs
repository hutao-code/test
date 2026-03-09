using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 课程注册数据访问接口，定义课程注册的持久化操作
    /// </summary>
    public interface ICourseRegistrationRepository
    {
        /// <summary>
        /// 注册学生到课程
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="courseId">课程ID</param>
        void Register(string studentId, string courseId);

        /// <summary>
        /// 取消学生的课程注册
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="courseId">课程ID</param>
        void Unregister(string studentId, string courseId);

        /// <summary>
        /// 获取学生的所有课程注册记录
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>课程注册记录集合</returns>
        IEnumerable<CourseRegistration> GetByStudent(string studentId);

        /// <summary>
        /// 获取课程的所有注册记录
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <returns>课程注册记录集合</returns>
        IEnumerable<CourseRegistration> GetByCourse(string courseId);

        /// <summary>
        /// 检查学生是否已注册某课程
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="courseId">课程ID</param>
        /// <returns>如果已注册返回 true，否则返回 false</returns>
        bool IsRegistered(string studentId, string courseId);

        /// <summary>
        /// 获取课程的注册人数
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <returns>注册人数</returns>
        int GetCourseRegistrationCount(string courseId);
    }
}
