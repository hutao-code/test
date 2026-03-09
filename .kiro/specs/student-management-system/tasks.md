# Implementation Plan: 学生管理系统

## Overview

本实施计划将学生管理系统的设计转化为可执行的编码任务。系统采用 ASP.NET MVC 5.2.3 架构，运行在 .NET Framework 4.8 上，使用三层架构模式（表示层、业务逻辑层、数据访问层）。

实施将按照以下顺序进行：
1. 建立数据模型和数据库架构
2. 实现数据访问层（Repository 模式）
3. 实现业务逻辑层（验证服务）
4. 实现表示层（控制器和视图）
5. 集成测试和错误处理
6. 安全性增强

每个任务都引用了相应的需求编号，确保完整的可追溯性。

## Tasks

- [x] 1. 设置项目结构和数据模型
  - 在 Models 文件夹中创建所有数据模型类（Student, Course, Grade, CourseRegistration）
  - 创建 ViewModels 文件夹并添加视图模型类（StudentViewModel, StudentSearchViewModel, StatisticsViewModel）
  - 添加数据注解（Data Annotations）进行模型验证
  - 创建数据库上下文类（StudentManagementContext）继承自 DbContext
  - _Requirements: 1.1, 2.1, 2.3, 2.4, 2.5, 2.6_

- [ ]* 1.1 为数据模型编写属性测试
  - **Property 1: Student CRUD Round-Trip**
  - **Validates: Requirements 1.1, 1.3**

- [x] 2. 创建数据库架构和迁移
  - 安装 Entity Framework 6.x NuGet 包
  - 配置 Web.config 中的数据库连接字符串
  - 创建数据库初始化器类，包含索引和约束定义
  - 使用 Code First 方式生成数据库架构（Students, Courses, Grades, CourseRegistrations 表）
  - 添加种子数据用于开发和测试
  - _Requirements: 8.1, 8.5_

- [ ] 3. 实现 Student Repository
  - [x] 3.1 创建 IStudentRepository 接口
    - 定义所有 CRUD 方法签名
    - 定义查询方法（Search, GetByGrade, GetByMajor, GetPaged）
    - 定义统计方法（GetTotalCount, GetCountByGrade, GetCountByMajor）
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 3.1, 3.3, 3.4, 3.5_

  - [x] 3.2 实现 StudentRepository 类
    - 实现基本 CRUD 操作（GetAll, GetById, Add, Update, Delete）
    - 实现查询方法，使用 LINQ 进行数据筛选和排序
    - 实现分页逻辑
    - 实现统计聚合方法
    - 添加事务支持（使用 TransactionScope）
    - 实现 DeleteWithRelatedData 方法，级联删除关联数据
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 8.1, 8.3, 8.4_

  - [ ]* 3.3 为 Student Repository 编写属性测试
    - **Property 2: Student Update Persistence**
    - **Property 3: Student Deletion Completeness**
    - **Property 30: Cascade Delete Completeness**
    - **Validates: Requirements 1.4, 1.5, 8.4**

  - [ ]* 3.4 为 Student Repository 编写单元测试
    - 测试空列表场景
    - 测试不存在的学生 ID 查询
    - 测试数据库连接失败场景（使用 Mock）
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_

- [ ] 4. 实现 Course 和 Registration Repositories
  - [x] 4.1 创建 ICourseRepository 和 ICourseRegistrationRepository 接口
    - 定义课程的基本 CRUD 方法
    - 定义课程注册方法（Register, Unregister, IsRegistered, GetCourseRegistrationCount）
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5_

  - [ ] 4.2 实现 CourseRepository 和 CourseRegistrationRepository 类
    - 实现课程的 CRUD 操作
    - 实现课程注册逻辑，包含重复注册检查
    - 实现课程容量检查逻辑
    - 添加注册时间戳记录
    - 使用事务确保注册操作的原子性
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6_

  - [ ]* 4.3 为课程注册编写属性测试
    - **Property 15: Course Registration Creation**
    - **Property 16: Duplicate Registration Prevention**
    - **Property 17: Course Unregistration Completeness**
    - **Property 18: Course Capacity Enforcement**
    - **Property 19: Registration Timestamp Invariant**
    - **Validates: Requirements 4.1, 4.2, 4.3, 4.4, 4.5, 4.6**

- [ ] 5. 实现 Grade Repository
  - [ ] 5.1 创建 IGradeRepository 接口并实现 GradeRepository 类
    - 实现 AddOrUpdate 方法（创建或更新成绩）
    - 实现按学生和课程查询成绩的方法
    - 实现 CalculateAverageGrade 方法
    - 实现 GetGradeDistribution 方法
    - 添加成绩时间戳记录（RecordedAt, UpdatedAt）
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6_

  - [ ]* 5.2 为成绩管理编写属性测试
    - **Property 20: Grade Record Round-Trip**
    - **Property 21: Grade Range Validation**
    - **Property 22: Course Grades Retrieval Correctness**
    - **Property 23: Average Grade Calculation Correctness**
    - **Property 24: Grade Timestamp Invariant**
    - **Validates: Requirements 5.1, 5.2, 5.3, 5.4, 5.5, 5.6**

- [ ] 6. Checkpoint - 确保数据访问层测试通过
  - 运行所有 Repository 单元测试和属性测试
  - 验证数据库操作的正确性
  - 如有问题请询问用户

- [ ] 7. 实现验证服务
  - [ ] 7.1 创建 IValidationService 接口和 ValidationResult 类
    - 定义 ValidateStudent, ValidateGrade, ValidateCourseRegistration 方法
    - 定义辅助验证方法（IsStudentIdUnique, IsValidEmail, IsValidStudentId）
    - 创建 ValidationResult 类包含 IsValid 和 Errors 属性
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6_

  - [ ] 7.2 实现 ValidationService 类
    - 实现 ValidateStudent 方法，检查所有必填字段、格式、范围和长度
    - 实现学号唯一性检查（调用 Repository）
    - 实现邮箱格式验证（使用正则表达式）
    - 实现学号格式验证（8位数字）
    - 实现 ValidateGrade 方法，检查成绩范围（0-100）
    - 实现 ValidateCourseRegistration 方法，检查重复注册和课程容量
    - 添加详细的验证错误消息
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 5.2_

  - [ ]* 7.3 为验证服务编写属性测试
    - **Property 4: Missing Required Field Rejection**
    - **Property 5: Duplicate Student ID Rejection**
    - **Property 6: Age Range Validation**
    - **Property 7: Name Length Validation**
    - **Property 8: Student ID Format Validation**
    - **Property 9: Email Format Validation**
    - **Validates: Requirements 2.1, 2.2, 2.3, 2.4, 2.5, 2.6**

  - [ ]* 7.4 为验证服务编写单元测试
    - 测试边界值（年龄 10 和 100）
    - 测试各种无效邮箱格式
    - 测试各种无效学号格式
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6_

- [ ] 8. 实现自定义异常类
  - 创建 ValidationException, NotFoundException, DuplicateStudentException, DataAccessException, CourseCapacityException 类
  - 每个异常类包含适当的构造函数和属性
  - _Requirements: 9.1, 9.2, 9.3_

- [ ] 9. 实现 StudentController - 基本 CRUD 操作
  - [ ] 9.1 创建 StudentController 类并配置依赖注入
    - 添加 IStudentRepository 和 IValidationService 依赖
    - 配置构造函数注入或属性注入
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_

  - [ ] 9.2 实现 Index 操作（学生列表）
    - 实现 GET Index 方法，支持分页（每页 20 条）
    - 调用 Repository 的 GetPaged 方法
    - 返回学生列表视图
    - _Requirements: 1.2, 3.5_

  - [ ] 9.3 实现 Details 操作（查看学生详情）
    - 实现 GET Details 方法，接受学生 ID 参数
    - 调用 Repository 的 GetById 方法
    - 处理学生不存在的情况（返回 404）
    - 返回学生详情视图
    - _Requirements: 1.3_

  - [ ] 9.4 实现 Create 操作（创建学生）
    - 实现 GET Create 方法，返回创建表单视图
    - 实现 POST Create 方法，接受 StudentViewModel 参数
    - 添加 [ValidateAntiForgeryToken] 属性防止 CSRF 攻击
    - 调用 ValidationService 验证输入
    - 如果验证通过，调用 Repository 的 Add 方法
    - 如果验证失败，返回视图并显示错误消息
    - _Requirements: 1.1, 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 10.3_

  - [ ] 9.5 实现 Edit 操作（更新学生）
    - 实现 GET Edit 方法，返回编辑表单视图
    - 实现 POST Edit 方法，接受 StudentViewModel 参数
    - 添加 [ValidateAntiForgeryToken] 属性
    - 调用 ValidationService 验证输入
    - 调用 Repository 的 Update 方法
    - _Requirements: 1.4, 10.3_

  - [ ] 9.6 实现 Delete 操作（删除学生）
    - 实现 POST Delete 方法，接受学生 ID 参数
    - 添加 [ValidateAntiForgeryToken] 属性
    - 调用 Repository 的 DeleteWithRelatedData 方法
    - 返回重定向到 Index
    - _Requirements: 1.5, 8.4_

- [ ] 10. 实现 StudentController - 查询和搜索操作
  - [ ] 10.1 实现 Search 操作
    - 实现 GET Search 方法，接受 keyword, grade, major, page, sortBy 参数
    - 根据参数调用相应的 Repository 查询方法
    - 支持按姓名关键字搜索（调用 Search 方法）
    - 支持按年级筛选（调用 GetByGrade 方法）
    - 支持按专业筛选（调用 GetByMajor 方法）
    - 支持排序（Name, StudentId, EnrollmentDate）
    - 支持分页
    - 返回 StudentSearchViewModel
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6_

  - [ ]* 10.2 为查询操作编写属性测试
    - **Property 10: Name Keyword Search Correctness**
    - **Property 11: Grade Filter Correctness**
    - **Property 12: Major Filter Correctness**
    - **Property 13: Pagination Correctness**
    - **Property 14: Sorting Correctness**
    - **Validates: Requirements 3.1, 3.3, 3.4, 3.5, 3.6**

- [ ] 11. 实现 StudentController - 课程和成绩操作
  - [ ] 11.1 实现 Courses 操作（查看学生课程）
    - 实现 GET Courses 方法，接受学生 ID 参数
    - 调用 Repository 的 GetStudentCourses 方法
    - 返回课程列表视图
    - _Requirements: 4.3_

  - [ ] 11.2 实现 Grades 操作（查看学生成绩）
    - 实现 GET Grades 方法，接受学生 ID 参数
    - 调用 GradeRepository 的 GetByStudent 方法
    - 调用 CalculateAverageGrade 方法计算平均成绩
    - 返回成绩单视图
    - _Requirements: 5.3, 5.5_

- [ ] 12. 实现 StudentController - 统计和报表操作
  - [ ] 12.1 实现 Statistics 操作
    - 实现 GET Statistics 方法
    - 调用 Repository 的统计方法（GetTotalCount, GetCountByGrade, GetCountByMajor）
    - 调用 CourseRegistrationRepository 获取课程注册统计
    - 调用 GradeRepository 获取成绩分布
    - 返回 StatisticsViewModel
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5_

  - [ ] 12.2 实现 ExportCsv 操作
    - 实现 GET ExportCsv 方法
    - 调用 Repository 的 GetAll 方法
    - 生成 CSV 格式的文件内容
    - 设置响应头（Content-Type: text/csv, Content-Disposition: attachment）
    - 返回 FileResult
    - _Requirements: 6.6_

  - [ ]* 12.3 为统计和导出编写属性测试
    - **Property 25: Grade Count Aggregation Correctness**
    - **Property 26: Major Count Aggregation Correctness**
    - **Property 27: Course Registration Count Correctness**
    - **Property 28: Grade Distribution Sum Correctness**
    - **Property 29: CSV Export Round-Trip**
    - **Validates: Requirements 6.2, 6.3, 6.4, 6.5, 6.6**

- [ ] 13. Checkpoint - 确保控制器基本功能测试通过
  - 运行所有控制器单元测试
  - 验证 CRUD 操作的正确性
  - 如有问题请询问用户

- [ ] 14. 实现错误处理和日志记录
  - [ ] 14.1 在 StudentController 中重写 OnException 方法
    - 捕获所有未处理的异常
    - 根据异常类型返回适当的错误视图或 HTTP 状态码
    - 生成唯一的跟踪 ID（使用 Guid）
    - 记录异常到 Application Insights
    - 向用户显示友好的错误消息，不暴露技术细节
    - _Requirements: 9.1, 9.4, 9.5, 9.6_

  - [ ] 14.2 在 Repository 类中添加错误处理
    - 在所有数据库操作中添加 try-catch 块
    - 捕获 DbUpdateException 并转换为自定义异常
    - 记录 SQL 错误代码和消息
    - 记录错误到 Application Insights
    - _Requirements: 8.2, 9.3_

  - [ ] 14.3 在 Global.asax.cs 中配置全局错误处理
    - 实现 Application_Error 方法
    - 记录所有未处理异常到 Application Insights
    - 记录请求 URL、UserAgent 和跟踪 ID
    - _Requirements: 9.1, 9.4_

  - [ ]* 14.4 为错误处理编写单元测试
    - 测试各种异常类型的处理
    - 验证错误消息的友好性
    - 验证跟踪 ID 的生成
    - _Requirements: 9.1, 9.2, 9.6_

- [ ] 15. 实现性能监控和日志记录
  - [ ] 15.1 配置 Application Insights
    - 验证 ApplicationInsights.config 配置
    - 在 Global.asax.cs 中初始化 TelemetryClient
    - 配置自动请求跟踪
    - _Requirements: 7.5, 9.4_

  - [ ] 15.2 添加性能日志记录
    - 在控制器操作中记录响应时间
    - 当响应时间超过阈值时记录性能警告（Index > 2s, Create/Edit > 1s, Search > 1.5s）
    - 为每个请求生成唯一的跟踪 ID
    - _Requirements: 7.1, 7.2, 7.3, 7.5, 7.6, 9.5_

  - [ ]* 15.3 为跟踪 ID 生成编写属性测试
    - **Property 31: Unique Request Tracking ID**
    - **Validates: Requirements 9.5**

- [ ] 16. 实现安全性增强
  - [ ] 16.1 添加输入验证和清理
    - 在所有控制器操作中验证输入参数
    - 使用 ASP.NET MVC 的内置 HTML 编码防止 XSS
    - 在 Razor 视图中使用 @Html.Encode() 或 @ 语法
    - _Requirements: 10.1, 10.2_

  - [ ] 16.2 确保使用参数化查询
    - 验证所有 Repository 方法使用 Entity Framework 的 LINQ 查询（自动参数化）
    - 避免使用字符串拼接构建 SQL 查询
    - _Requirements: 10.1, 10.6_

  - [ ] 16.3 添加 CSRF 保护
    - 验证所有 POST 操作都有 [ValidateAntiForgeryToken] 属性
    - 在所有表单视图中添加 @Html.AntiForgeryToken()
    - _Requirements: 10.3_

  - [ ] 16.4 添加授权检查（可选，如果需求中有角色管理）
    - 添加 [Authorize] 属性到需要授权的操作
    - 实现自定义授权逻辑（如果需要）
    - 返回 403 禁止访问响应给未授权用户
    - _Requirements: 10.4, 10.5_

  - [ ]* 16.5 为安全性编写属性测试
    - **Property 32: SQL Injection Prevention**
    - **Property 33: XSS Prevention**
    - **Validates: Requirements 10.1, 10.2, 10.6**

- [ ] 17. 创建 Razor 视图
  - [ ] 17.1 创建学生管理视图
    - 创建 Views/Student/Index.cshtml（学生列表，包含分页控件）
    - 创建 Views/Student/Details.cshtml（学生详情）
    - 创建 Views/Student/Create.cshtml（创建表单，包含验证消息和 AntiForgeryToken）
    - 创建 Views/Student/Edit.cshtml（编辑表单）
    - 创建 Views/Student/Search.cshtml（搜索表单和结果列表）
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 3.1, 3.2, 3.3, 3.4_

  - [ ] 17.2 创建课程和成绩视图
    - 创建 Views/Student/Courses.cshtml（学生课程列表）
    - 创建 Views/Student/Grades.cshtml（学生成绩单，显示平均成绩）
    - _Requirements: 4.3, 5.3, 5.5_

  - [ ] 17.3 创建统计和错误视图
    - 创建 Views/Student/Statistics.cshtml（统计报表，使用图表或表格）
    - 创建 Views/Shared/Error.cshtml（通用错误页面，显示跟踪 ID）
    - 创建 Views/Shared/ValidationError.cshtml（验证错误页面）
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5, 9.6_

  - [ ] 17.4 更新共享布局
    - 更新 Views/Shared/_Layout.cshtml，添加学生管理导航链接
    - 添加 Application Insights JavaScript SDK（如果需要客户端监控）
    - _Requirements: 7.5_

- [ ] 18. 配置路由
  - 在 App_Start/RouteConfig.cs 中添加学生管理相关路由
  - 配置默认路由指向 Student/Index（如果需要）
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_

- [ ] 19. 集成测试
  - [ ]* 19.1 编写端到端集成测试
    - 测试完整的学生生命周期（创建、读取、更新、删除）
    - 测试课程注册流程
    - 测试成绩录入和查询流程
    - 测试搜索和筛选功能
    - 测试统计报表生成
    - 使用内存数据库或测试数据库
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 4.1, 4.3, 4.4, 5.1, 5.3, 3.1, 3.3, 3.4, 6.1_

- [ ] 20. 最终检查点 - 确保所有测试通过
  - 运行所有单元测试、属性测试和集成测试
  - 验证代码覆盖率达到 80% 以上
  - 检查所有 33 个设计属性都有对应的属性测试
  - 手动测试主要用户流程
  - 检查 Application Insights 是否正确记录遥测数据
  - 如有问题请询问用户

## Notes

- 标记为 `*` 的任务是可选的，可以跳过以加快 MVP 开发
- 每个任务都引用了具体的需求编号，确保可追溯性
- Checkpoint 任务确保增量验证
- 属性测试验证通用正确性属性
- 单元测试验证特定示例和边缘情况
- 所有 POST 操作必须包含 CSRF 保护
- 所有数据库操作必须使用参数化查询防止 SQL 注入
- 所有用户输入必须进行验证和清理
