using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    /// <summary>
    /// 课程数据访问接口，定义课程数据的持久化操作
    /// </summary>
    public interface ICourseRepository
    {
        // 基本 CRUD 操作

        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns>课程集合</returns>
        IEnumerable<Course> GetAll();

        /// <summary>
        /// 根据课程ID获取课程
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <returns>课程对象，如果不存在则返回 null</returns>
        Course GetById(string courseId);

        /// <summary>
        /// 添加新课程
        /// </summary>
        /// <param name="course">课程对象</param>
        void Add(Course course);

        /// <summary>
        /// 更新课程信息
        /// </summary>
        /// <param name="course">课程对象</param>
        void Update(Course course);

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseId">课程ID</param>
        void Delete(string courseId);
    }
}
