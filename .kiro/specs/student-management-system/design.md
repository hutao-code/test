# 设计文档：学生管理系统

## Overview

学生管理系统是一个基于 ASP.NET MVC 5.2.3 的 Web 应用程序模块，运行在 .NET Framework 4.8 上。该系统提供完整的学生信息管理功能，包括学生基本信息的 CRUD 操作、课程注册管理、成绩录入与查询，以及数据统计报表功能。

系统采用经典的三层架构模式：
- **表示层（Presentation Layer）**: 使用 ASP.NET MVC 控制器和 Razor 视图处理用户交互
- **业务逻辑层（Business Logic Layer）**: 包含验证服务和业务规则处理
- **数据访问层（Data Access Layer）**: 使用 Repository 模式封装数据持久化操作

系统集成 Application Insights 进行性能监控和错误跟踪，确保生产环境的可观测性。

## Architecture

### 架构模式

系统采用 **MVC (Model-View-Controller)** 架构模式，结合 **Repository 模式** 和 **Service 层**：

```
┌─────────────────────────────────────────────────────────────┐
│                        Presentation Layer                    │
│  ┌──────────────────┐         ┌─────────────────────────┐  │
│  │  StudentController│         │   Razor Views (.cshtml) │  │
│  │  - Index()        │────────▶│   - Index.cshtml        │  │
│  │  - Create()       │         │   - Create.cshtml       │  │
│  │  - Edit()         │         │   - Edit.cshtml         │  │
│  │  - Delete()       │         │   - Details.cshtml      │  │
│  │  - Search()       │         │   - Search.cshtml       │  │
│  └──────────────────┘         └─────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                     Business Logic Layer                     │
│  ┌──────────────────────────────────────────────────────┐  │
│  │            ValidationService                          │  │
│  │  - ValidateStudent(Student student)                  │  │
│  │  - ValidateGrade(Grade grade)                        │  │
│  │  - ValidateCourseRegistration(...)                   │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      Data Access Layer                       │
│  ┌──────────────────┐  ┌──────────────────┐               │
│  │ StudentRepository│  │ CourseRepository │               │
│  │ - GetAll()       │  │ - GetAll()       │               │
│  │ - GetById(id)    │  │ - GetById(id)    │               │
│  │ - Add(student)   │  │ - Add(course)    │               │
│  │ - Update(...)    │  │ - Update(...)    │               │
│  │ - Delete(id)     │  │ - Delete(id)     │               │
│  └──────────────────┘  └──────────────────┘               │
│                                                              │
│  ┌──────────────────┐  ┌──────────────────┐               │
│  │  GradeRepository │  │ RegistrationRepo │               │
│  │ - GetByStudent() │  │ - Register(...)  │               │
│  │ - GetByCourse()  │  │ - Unregister()   │               │
│  │ - AddOrUpdate()  │  │ - GetByStudent() │               │
│  └──────────────────┘  └──────────────────┘               │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      Database (SQL Server)                   │
│  Tables: Students, Courses, Grades, CourseRegistrations     │
└─────────────────────────────────────────────────────────────┘
```

### 关键设计决策

1. **Repository 模式**: 将数据访问逻辑与业务逻辑分离，便于单元测试和未来的数据源切换
2. **验证服务集中化**: 所有输入验证逻辑集中在 ValidationService 中，确保验证规则的一致性
3. **事务管理**: 在 Repository 层使用 TransactionScope 确保复杂操作的原子性
4. **异步操作**: 对于耗时的数据库操作，考虑使用异步方法提高响应性（虽然 .NET Framework 4.8 支持有限）
5. **Application Insights 集成**: 在 Global.asax 中配置，自动跟踪所有 HTTP 请求和异常

## Components and Interfaces

### 1. StudentController

MVC 控制器，处理所有学生相关的 HTTP 请求。

```csharp
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepository;
    private readonly IValidationService _validationService;
    
    // GET: Student
    public ActionResult Index(int page = 1, int pageSize = 20);
    
    // GET: Student/Details/5
    public ActionResult Details(string id);
    
    // GET: Student/Create
    public ActionResult Create();
    
    // POST: Student/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(StudentViewModel model);
    
    // GET: Student/Edit/5
    public ActionResult Edit(string id);
    
    // POST: Student/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(StudentViewModel model);
    
    // POST: Student/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(string id);
    
    // GET: Student/Search
    public ActionResult Search(string keyword, string grade, string major, 
                               int page = 1, string sortBy = "Name");
    
    // GET: Student/Courses/5
    public ActionResult Courses(string id);
    
    // GET: Student/Grades/5
    public ActionResult Grades(string id);
    
    // GET: Student/Statistics
    public ActionResult Statistics();
    
    // GET: Student/ExportCsv
    public ActionResult ExportCsv();
}
```

### 2. IStudentRepository

数据访问接口，定义学生数据的持久化操作。

```csharp
public interface IStudentRepository
{
    // 基本 CRUD 操作
    IEnumerable<Student> GetAll();
    Student GetById(string studentId);
    void Add(Student student);
    void Update(Student student);
    void Delete(string studentId);
    
    // 查询操作
    IEnumerable<Student> Search(string keyword);
    IEnumerable<Student> GetByGrade(string grade);
    IEnumerable<Student> GetByMajor(string major);
    IEnumerable<Student> GetPaged(int page, int pageSize, string sortBy);
    
    // 统计操作
    int GetTotalCount();
    Dictionary<string, int> GetCountByGrade();
    Dictionary<string, int> GetCountByMajor();
    
    // 关联操作
    IEnumerable<Course> GetStudentCourses(string studentId);
    IEnumerable<Grade> GetStudentGrades(string studentId);
    
    // 事务支持
    void DeleteWithRelatedData(string studentId);
}
```

### 3. IValidationService

验证服务接口，负责所有业务规则验证。

```csharp
public interface IValidationService
{
    ValidationResult ValidateStudent(Student student);
    ValidationResult ValidateGrade(Grade grade);
    ValidationResult ValidateCourseRegistration(string studentId, string courseId);
    bool IsStudentIdUnique(string studentId);
    bool IsValidEmail(string email);
    bool IsValidStudentId(string studentId);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
}
```

### 4. ICourseRegistrationRepository

课程注册数据访问接口。

```csharp
public interface ICourseRegistrationRepository
{
    void Register(string studentId, string courseId);
    void Unregister(string studentId, string courseId);
    IEnumerable<CourseRegistration> GetByStudent(string studentId);
    IEnumerable<CourseRegistration> GetByCourse(string courseId);
    bool IsRegistered(string studentId, string courseId);
    int GetCourseRegistrationCount(string courseId);
}
```

### 5. IGradeRepository

成绩数据访问接口。

```csharp
public interface IGradeRepository
{
    void AddOrUpdate(Grade grade);
    IEnumerable<Grade> GetByStudent(string studentId);
    IEnumerable<Grade> GetByCourse(string courseId);
    Grade GetByStudentAndCourse(string studentId, string courseId);
    decimal CalculateAverageGrade(string studentId);
    Dictionary<string, int> GetGradeDistribution(string courseId);
}
```

## Data Models

### Student Model

```csharp
public class Student
{
    [Required]
    [StringLength(8, MinimumLength = 8)]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "学号必须是8位数字")]
    public string StudentId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [Range(10, 100)]
    public int Age { get; set; }
    
    [Required]
    public string Gender { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Major { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Grade { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    
    public DateTime EnrollmentDate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    // 导航属性
    public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
}
```

### Course Model

```csharp
public class Course
{
    [Required]
    [StringLength(20)]
    public string CourseId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string CourseName { get; set; }
    
    [Required]
    public int Credits { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Instructor { get; set; }
    
    [Required]
    public int MaxStudents { get; set; }
    
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // 导航属性
    public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
}
```

### Grade Model

```csharp
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
```

### CourseRegistration Model

```csharp
public class CourseRegistration
{
    public int RegistrationId { get; set; }
    
    [Required]
    [StringLength(8)]
    public string StudentId { get; set; }
    
    [Required]
    [StringLength(20)]
    public string CourseId { get; set; }
    
    public DateTime RegisteredAt { get; set; }
    
    // 导航属性
    public virtual Student Student { get; set; }
    public virtual Course Course { get; set; }
}
```

### View Models

```csharp
public class StudentViewModel
{
    public string StudentId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Major { get; set; }
    public string Grade { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

public class StudentSearchViewModel
{
    public string Keyword { get; set; }
    public string Grade { get; set; }
    public string Major { get; set; }
    public string SortBy { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<Student> Results { get; set; }
}

public class StatisticsViewModel
{
    public int TotalStudents { get; set; }
    public Dictionary<string, int> StudentsByGrade { get; set; }
    public Dictionary<string, int> StudentsByMajor { get; set; }
    public Dictionary<string, int> CourseRegistrationCounts { get; set; }
}
```

### Database Schema

```sql
-- Students 表
CREATE TABLE Students (
    StudentId NVARCHAR(8) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Age INT NOT NULL CHECK (Age BETWEEN 10 AND 100),
    Gender NVARCHAR(10) NOT NULL,
    Major NVARCHAR(50) NOT NULL,
    Grade NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    EnrollmentDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME
);

CREATE INDEX IX_Students_Name ON Students(Name);
CREATE INDEX IX_Students_Grade ON Students(Grade);
CREATE INDEX IX_Students_Major ON Students(Major);

-- Courses 表
CREATE TABLE Courses (
    CourseId NVARCHAR(20) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    Credits INT NOT NULL,
    Instructor NVARCHAR(50) NOT NULL,
    MaxStudents INT NOT NULL,
    Description NVARCHAR(500),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- CourseRegistrations 表
CREATE TABLE CourseRegistrations (
    RegistrationId INT PRIMARY KEY IDENTITY(1,1),
    StudentId NVARCHAR(8) NOT NULL,
    CourseId NVARCHAR(20) NOT NULL,
    RegisteredAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE,
    UNIQUE (StudentId, CourseId)
);

CREATE INDEX IX_CourseRegistrations_StudentId ON CourseRegistrations(StudentId);
CREATE INDEX IX_CourseRegistrations_CourseId ON CourseRegistrations(CourseId);

-- Grades 表
CREATE TABLE Grades (
    GradeId INT PRIMARY KEY IDENTITY(1,1),
    StudentId NVARCHAR(8) NOT NULL,
    CourseId NVARCHAR(20) NOT NULL,
    Score DECIMAL(5,2) NOT NULL CHECK (Score BETWEEN 0 AND 100),
    RecordedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE,
    UNIQUE (StudentId, CourseId)
);

CREATE INDEX IX_Grades_StudentId ON Grades(StudentId);
CREATE INDEX IX_Grades_CourseId ON Grades(CourseId);
```


## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system-essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

### Property 1: Student CRUD Round-Trip

*For any* valid student object, creating the student and then retrieving it by ID should return an equivalent student object with all fields preserved.

**Validates: Requirements 1.1, 1.3**

### Property 2: Student Update Persistence

*For any* existing student and any valid update data, updating the student and then retrieving it should return the student with the updated values.

**Validates: Requirements 1.4**

### Property 3: Student Deletion Completeness

*For any* student ID, after deleting that student, attempting to retrieve the student should return null or not found.

**Validates: Requirements 1.5**

### Property 4: Missing Required Field Rejection

*For any* student object with one or more required fields set to null or empty, validation should fail and return an error indicating which fields are missing.

**Validates: Requirements 2.1**

### Property 5: Duplicate Student ID Rejection

*For any* student ID that already exists in the system, attempting to create a new student with that ID should fail validation and return a duplicate error.

**Validates: Requirements 2.2**

### Property 6: Age Range Validation

*For any* integer value outside the range [10, 100], using that value as a student's age should fail validation and return a range error.

**Validates: Requirements 2.3**

### Property 7: Name Length Validation

*For any* string longer than 50 characters, using that string as a student's name should fail validation and return a length error.

**Validates: Requirements 2.4**

### Property 8: Student ID Format Validation

*For any* string that does not match the pattern of exactly 8 digits, using that string as a student ID should fail validation and return a format error.

**Validates: Requirements 2.5**

### Property 9: Email Format Validation

*For any* string that does not conform to valid email format, using that string as a student's email should fail validation and return a format error.

**Validates: Requirements 2.6**

### Property 10: Name Keyword Search Correctness

*For any* keyword string and any set of students in the system, searching by that keyword should return only students whose names contain that keyword (case-insensitive).

**Validates: Requirements 3.1**

### Property 11: Grade Filter Correctness

*For any* grade value and any set of students in the system, filtering by that grade should return only students whose grade field exactly matches that value.

**Validates: Requirements 3.3**

### Property 12: Major Filter Correctness

*For any* major value and any set of students in the system, filtering by that major should return only students whose major field exactly matches that value.

**Validates: Requirements 3.4**

### Property 13: Pagination Correctness

*For any* page number and page size of 20, the returned results should contain at most 20 students, and the students should be the correct subset based on the page number and total count.

**Validates: Requirements 3.5**

### Property 14: Sorting Correctness

*For any* sort field (Name, StudentId, or EnrollmentDate) and any set of students, the returned list should be ordered according to that field in ascending order.

**Validates: Requirements 3.6**

### Property 15: Course Registration Creation

*For any* valid student ID and course ID where the student is not already registered, registering the student for the course should create an association that can be retrieved when querying the student's courses.

**Validates: Requirements 4.1, 4.3**

### Property 16: Duplicate Registration Prevention

*For any* student-course pair where the student is already registered for that course, attempting to register again should fail and return an error.

**Validates: Requirements 4.2**

### Property 17: Course Unregistration Completeness

*For any* existing course registration, unregistering the student from the course should remove the association such that the course no longer appears in the student's course list.

**Validates: Requirements 4.4**

### Property 18: Course Capacity Enforcement

*For any* course that has reached its maximum student capacity, attempting to register an additional student should fail and return a capacity error.

**Validates: Requirements 4.5**

### Property 19: Registration Timestamp Invariant

*For any* course registration created in the system, the registration record should have a non-null RegisteredAt timestamp.

**Validates: Requirements 4.6**

### Property 20: Grade Record Round-Trip

*For any* valid student ID, course ID, and score value between 0 and 100, submitting a grade and then retrieving grades for that student-course pair should return the submitted score.

**Validates: Requirements 5.1, 5.3**

### Property 21: Grade Range Validation

*For any* decimal value outside the range [0, 100], using that value as a grade score should fail validation and return a range error.

**Validates: Requirements 5.2**

### Property 22: Course Grades Retrieval Correctness

*For any* course ID and any set of grades in the system, retrieving grades by course should return only grades associated with that specific course.

**Validates: Requirements 5.4**

### Property 23: Average Grade Calculation Correctness

*For any* student with one or more grade records, the calculated average grade should equal the mathematical mean of all the student's scores.

**Validates: Requirements 5.5**

### Property 24: Grade Timestamp Invariant

*For any* grade record created or updated in the system, the record should have a non-null RecordedAt timestamp, and if updated, a non-null UpdatedAt timestamp.

**Validates: Requirements 5.6**

### Property 25: Grade Count Aggregation Correctness

*For any* set of students in the system, the sum of student counts across all grades should equal the total number of students.

**Validates: Requirements 6.2**

### Property 26: Major Count Aggregation Correctness

*For any* set of students in the system, the sum of student counts across all majors should equal the total number of students.

**Validates: Requirements 6.3**

### Property 27: Course Registration Count Correctness

*For any* course and any set of registrations in the system, the registration count for that course should equal the actual number of student-course associations for that course.

**Validates: Requirements 6.4**

### Property 28: Grade Distribution Sum Correctness

*For any* set of grades in the system, the sum of student counts across all grade ranges should equal the total number of grade records.

**Validates: Requirements 6.5**

### Property 29: CSV Export Round-Trip

*For any* set of students in the system, exporting to CSV and then parsing the CSV should produce student records with all essential fields (StudentId, Name, Age, Gender, Major, Grade) preserved.

**Validates: Requirements 6.6**

### Property 30: Cascade Delete Completeness

*For any* student with associated course registrations and grade records, deleting the student should also remove all associated registrations and grades such that queries for those associations return empty results.

**Validates: Requirements 8.4**

### Property 31: Unique Request Tracking ID

*For any* HTTP request processed by the system, a unique tracking ID should be generated and associated with that request for logging purposes.

**Validates: Requirements 9.5**

### Property 32: SQL Injection Prevention

*For any* input string containing SQL syntax characters (such as single quotes, semicolons, or SQL keywords), submitting that input should not result in SQL syntax errors or unintended database operations.

**Validates: Requirements 10.1, 10.6**

### Property 33: XSS Prevention

*For any* input string containing HTML or JavaScript tags (such as `<script>`, `<img>`, or event handlers), the output rendered in views should have these tags properly escaped or sanitized such that they are not executed as code.

**Validates: Requirements 10.2**


## Error Handling

### Error Handling Strategy

系统采用分层错误处理策略，确保错误在适当的层级被捕获和处理：

#### 1. Controller Layer Error Handling

```csharp
public class StudentController : Controller
{
    protected override void OnException(ExceptionContext filterContext)
    {
        // 记录异常到 Application Insights
        var telemetry = new TelemetryClient();
        telemetry.TrackException(filterContext.Exception);
        
        // 生成唯一的跟踪 ID
        var trackingId = Guid.NewGuid().ToString();
        ViewBag.TrackingId = trackingId;
        
        // 根据异常类型返回适当的错误视图
        if (filterContext.Exception is ValidationException)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "ValidationError",
                ViewData = new ViewDataDictionary(filterContext.Exception)
            };
        }
        else if (filterContext.Exception is NotFoundException)
        {
            filterContext.Result = new HttpNotFoundResult();
        }
        else
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(new { TrackingId = trackingId })
            };
        }
        
        filterContext.ExceptionHandled = true;
    }
}
```

#### 2. Repository Layer Error Handling

```csharp
public class StudentRepository : IStudentRepository
{
    public void Add(Student student)
    {
        try
        {
            using (var transaction = new TransactionScope())
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                transaction.Complete();
            }
        }
        catch (DbUpdateException ex)
        {
            // 记录数据库错误
            _logger.Error($"Database error adding student {student.StudentId}", ex);
            
            // 检查是否是唯一约束违反
            if (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                throw new DuplicateStudentException($"Student ID {student.StudentId} already exists", ex);
            }
            
            throw new DataAccessException("Failed to add student", ex);
        }
        catch (Exception ex)
        {
            _logger.Error($"Unexpected error adding student {student.StudentId}", ex);
            throw;
        }
    }
}
```

#### 3. Validation Service Error Handling

```csharp
public class ValidationService : IValidationService
{
    public ValidationResult ValidateStudent(Student student)
    {
        var result = new ValidationResult { IsValid = true, Errors = new List<string>() };
        
        try
        {
            // 必填字段验证
            if (string.IsNullOrWhiteSpace(student.StudentId))
                result.Errors.Add("学号不能为空");
            
            if (string.IsNullOrWhiteSpace(student.Name))
                result.Errors.Add("姓名不能为空");
            
            // 格式验证
            if (!Regex.IsMatch(student.StudentId ?? "", @"^\d{8}$"))
                result.Errors.Add("学号必须是8位数字");
            
            // 范围验证
            if (student.Age < 10 || student.Age > 100)
                result.Errors.Add("年龄必须在10到100之间");
            
            // 长度验证
            if (student.Name?.Length > 50)
                result.Errors.Add("姓名长度不能超过50个字符");
            
            // 邮箱验证
            if (!string.IsNullOrEmpty(student.Email) && !IsValidEmail(student.Email))
                result.Errors.Add("邮箱格式无效");
            
            result.IsValid = result.Errors.Count == 0;
            
            // 记录验证失败
            if (!result.IsValid)
            {
                _logger.Warn($"Validation failed for student {student.StudentId}: {string.Join(", ", result.Errors)}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Error during validation", ex);
            result.IsValid = false;
            result.Errors.Add("验证过程中发生错误");
        }
        
        return result;
    }
}
```

### Custom Exception Types

```csharp
public class ValidationException : Exception
{
    public List<string> ValidationErrors { get; set; }
    
    public ValidationException(string message, List<string> errors) : base(message)
    {
        ValidationErrors = errors;
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class DuplicateStudentException : Exception
{
    public DuplicateStudentException(string message, Exception innerException) 
        : base(message, innerException) { }
}

public class DataAccessException : Exception
{
    public DataAccessException(string message, Exception innerException) 
        : base(message, innerException) { }
}

public class CourseCapacityException : Exception
{
    public CourseCapacityException(string message) : base(message) { }
}
```

### Error Logging Configuration

在 `Global.asax.cs` 中配置全局错误处理：

```csharp
protected void Application_Error(object sender, EventArgs e)
{
    var exception = Server.GetLastError();
    var httpContext = ((HttpApplication)sender).Context;
    
    // 记录到 Application Insights
    var telemetry = new TelemetryClient();
    telemetry.TrackException(exception, new Dictionary<string, string>
    {
        { "Url", httpContext.Request.Url.ToString() },
        { "UserAgent", httpContext.Request.UserAgent },
        { "TrackingId", Guid.NewGuid().ToString() }
    });
    
    // 记录到本地日志
    _logger.Error("Unhandled exception", exception);
}
```

### User-Friendly Error Messages

系统确保向用户显示友好的错误消息，而不暴露技术细节：

- **验证错误**: 显示具体的验证失败原因（如"学号必须是8位数字"）
- **未找到错误**: 显示"未找到指定的学生"而不是数据库查询细节
- **系统错误**: 显示"系统遇到错误，请稍后重试"并提供跟踪 ID 用于支持查询
- **权限错误**: 显示"您没有权限执行此操作"

## Testing Strategy

### 测试方法概述

系统采用**双重测试策略**，结合单元测试和基于属性的测试（Property-Based Testing, PBT）：

- **单元测试**: 验证特定示例、边缘情况和错误条件
- **属性测试**: 通过随机生成的输入验证通用属性，确保系统在各种输入下的正确性

两种测试方法互补，共同提供全面的测试覆盖：
- 单元测试捕获具体的错误和已知的边缘情况
- 属性测试验证通用正确性并发现未预期的边缘情况

### Property-Based Testing 配置

**测试框架选择**: 使用 **FsCheck** 库（.NET 平台的属性测试框架）

**配置要求**:
- 每个属性测试最少运行 **100 次迭代**（由于随机化）
- 每个测试必须引用设计文档中的对应属性
- 标签格式: `// Feature: student-management-system, Property {number}: {property_text}`

**安装 FsCheck**:
```bash
Install-Package FsCheck
Install-Package FsCheck.Xunit  # 如果使用 xUnit
```

### 测试组织结构

```
WebApplication1.Tests/
├── Unit/
│   ├── Controllers/
│   │   └── StudentControllerTests.cs
│   ├── Services/
│   │   └── ValidationServiceTests.cs
│   └── Repositories/
│       └── StudentRepositoryTests.cs
├── Properties/
│   ├── StudentCrudPropertiesTests.cs
│   ├── ValidationPropertiesTests.cs
│   ├── QueryPropertiesTests.cs
│   ├── CourseRegistrationPropertiesTests.cs
│   ├── GradeManagementPropertiesTests.cs
│   └── SecurityPropertiesTests.cs
└── Integration/
    └── StudentManagementIntegrationTests.cs
```

### Property-Based Testing 示例

#### 示例 1: Student CRUD Round-Trip Property

```csharp
// Feature: student-management-system, Property 1: Student CRUD Round-Trip
[Property(MaxTest = 100)]
public Property StudentCrudRoundTrip()
{
    return Prop.ForAll(
        StudentGenerators.ValidStudent(),
        student =>
        {
            // Arrange
            var repository = new StudentRepository(_context);
            
            // Act
            repository.Add(student);
            var retrieved = repository.GetById(student.StudentId);
            
            // Assert
            return retrieved != null &&
                   retrieved.StudentId == student.StudentId &&
                   retrieved.Name == student.Name &&
                   retrieved.Age == student.Age &&
                   retrieved.Gender == student.Gender &&
                   retrieved.Major == student.Major &&
                   retrieved.Grade == student.Grade &&
                   retrieved.Email == student.Email;
        });
}
```

#### 示例 2: Age Range Validation Property

```csharp
// Feature: student-management-system, Property 6: Age Range Validation
[Property(MaxTest = 100)]
public Property AgeRangeValidation()
{
    return Prop.ForAll(
        Arb.From(Gen.Choose(-100, 9).Or(Gen.Choose(101, 200))),
        invalidAge =>
        {
            // Arrange
            var student = StudentGenerators.ValidStudent().Sample(1, 1).First();
            student.Age = invalidAge;
            var validationService = new ValidationService();
            
            // Act
            var result = validationService.ValidateStudent(student);
            
            // Assert
            return !result.IsValid &&
                   result.Errors.Any(e => e.Contains("年龄") || e.Contains("age"));
        });
}
```

#### 示例 3: Name Keyword Search Property

```csharp
// Feature: student-management-system, Property 10: Name Keyword Search Correctness
[Property(MaxTest = 100)]
public Property NameKeywordSearchCorrectness()
{
    return Prop.ForAll(
        StudentGenerators.StudentList(),
        Gen.Elements("张", "李", "王", "test", "student"),
        (students, keyword) =>
        {
            // Arrange
            var repository = new StudentRepository(_context);
            foreach (var student in students)
            {
                repository.Add(student);
            }
            
            // Act
            var results = repository.Search(keyword);
            
            // Assert
            return results.All(s => s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        });
}
```

### FsCheck Generators

创建自定义生成器以生成有效的测试数据：

```csharp
public static class StudentGenerators
{
    public static Gen<Student> ValidStudent()
    {
        return from id in Gen.Choose(10000000, 99999999).Select(n => n.ToString())
               from name in Gen.Elements("张三", "李四", "王五", "赵六", "Test Student")
               from age in Gen.Choose(10, 100)
               from gender in Gen.Elements("男", "女")
               from major in Gen.Elements("计算机科学", "软件工程", "数据科学", "信息安全")
               from grade in Gen.Elements("2020级", "2021级", "2022级", "2023级")
               from email in Gen.Elements("test@example.com", "student@university.edu")
               select new Student
               {
                   StudentId = id,
                   Name = name,
                   Age = age,
                   Gender = gender,
                   Major = major,
                   Grade = grade,
                   Email = email,
                   EnrollmentDate = DateTime.Now.AddYears(-2),
                   CreatedAt = DateTime.Now
               };
    }
    
    public static Gen<List<Student>> StudentList()
    {
        return Gen.ListOf(10, ValidStudent());
    }
    
    public static Gen<string> InvalidStudentId()
    {
        return Gen.OneOf(
            Gen.Constant(""),
            Gen.Constant("123"),
            Gen.Constant("abcdefgh"),
            Gen.Constant("1234567"),
            Gen.Constant("123456789")
        );
    }
}
```

### Unit Testing 策略

单元测试专注于：

1. **特定示例**: 测试已知的输入-输出对
2. **边缘情况**: 空列表、空字符串、边界值
3. **错误条件**: 数据库连接失败、验证失败、权限拒绝
4. **集成点**: 控制器与服务之间的交互

#### 单元测试示例

```csharp
[TestClass]
public class StudentControllerTests
{
    [TestMethod]
    public void Create_WithValidStudent_ReturnsRedirectToIndex()
    {
        // Arrange
        var mockRepo = new Mock<IStudentRepository>();
        var mockValidation = new Mock<IValidationService>();
        mockValidation.Setup(v => v.ValidateStudent(It.IsAny<Student>()))
                     .Returns(new ValidationResult { IsValid = true });
        
        var controller = new StudentController(mockRepo.Object, mockValidation.Object);
        var model = new StudentViewModel
        {
            StudentId = "12345678",
            Name = "Test Student",
            Age = 20,
            Gender = "男",
            Major = "计算机科学",
            Grade = "2023级",
            Email = "test@example.com"
        };
        
        // Act
        var result = controller.Create(model) as RedirectToRouteResult;
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.RouteValues["action"]);
        mockRepo.Verify(r => r.Add(It.IsAny<Student>()), Times.Once);
    }
    
    [TestMethod]
    public void Create_WithInvalidStudent_ReturnsViewWithErrors()
    {
        // Arrange
        var mockRepo = new Mock<IStudentRepository>();
        var mockValidation = new Mock<IValidationService>();
        mockValidation.Setup(v => v.ValidateStudent(It.IsAny<Student>()))
                     .Returns(new ValidationResult 
                     { 
                         IsValid = false, 
                         Errors = new List<string> { "学号格式无效" }
                     });
        
        var controller = new StudentController(mockRepo.Object, mockValidation.Object);
        var model = new StudentViewModel { StudentId = "invalid" };
        
        // Act
        var result = controller.Create(model) as ViewResult;
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(controller.ModelState.IsValid);
        mockRepo.Verify(r => r.Add(It.IsAny<Student>()), Times.Never);
    }
    
    [TestMethod]
    public void Delete_WithExistingStudent_RemovesStudentAndRelatedData()
    {
        // Arrange
        var mockRepo = new Mock<IStudentRepository>();
        mockRepo.Setup(r => r.GetById("12345678"))
               .Returns(new Student { StudentId = "12345678" });
        
        var controller = new StudentController(mockRepo.Object, null);
        
        // Act
        var result = controller.Delete("12345678");
        
        // Assert
        mockRepo.Verify(r => r.DeleteWithRelatedData("12345678"), Times.Once);
    }
}
```

### Integration Testing

集成测试验证系统各组件之间的交互：

```csharp
[TestClass]
public class StudentManagementIntegrationTests
{
    private TestContext _context;
    
    [TestInitialize]
    public void Setup()
    {
        // 使用内存数据库或测试数据库
        _context = new TestContext();
    }
    
    [TestMethod]
    public void CompleteStudentLifecycle_CreateUpdateDelete_WorksCorrectly()
    {
        // Arrange
        var repository = new StudentRepository(_context);
        var validationService = new ValidationService(repository);
        var controller = new StudentController(repository, validationService);
        
        var student = new StudentViewModel
        {
            StudentId = "12345678",
            Name = "Integration Test",
            Age = 20,
            Gender = "男",
            Major = "计算机科学",
            Grade = "2023级"
        };
        
        // Act & Assert - Create
        var createResult = controller.Create(student);
        Assert.IsInstanceOfType(createResult, typeof(RedirectToRouteResult));
        
        // Act & Assert - Read
        var detailsResult = controller.Details("12345678") as ViewResult;
        Assert.IsNotNull(detailsResult);
        var retrievedStudent = detailsResult.Model as Student;
        Assert.AreEqual("Integration Test", retrievedStudent.Name);
        
        // Act & Assert - Update
        student.Name = "Updated Name";
        var updateResult = controller.Edit(student);
        Assert.IsInstanceOfType(updateResult, typeof(RedirectToRouteResult));
        
        var updatedStudent = repository.GetById("12345678");
        Assert.AreEqual("Updated Name", updatedStudent.Name);
        
        // Act & Assert - Delete
        controller.Delete("12345678");
        var deletedStudent = repository.GetById("12345678");
        Assert.IsNull(deletedStudent);
    }
}
```

### Test Coverage Goals

- **代码覆盖率目标**: 最低 80%
- **属性测试覆盖**: 所有 33 个设计属性必须有对应的属性测试
- **单元测试覆盖**: 所有公共方法和关键私有方法
- **集成测试覆盖**: 主要用户流程和跨组件交互

### Continuous Integration

在 CI/CD 管道中集成测试：

```bash
# 运行所有测试
msbuild WebApplication1.sln /t:Build /p:Configuration=Release
vstest.console.exe WebApplication1.Tests\bin\Release\WebApplication1.Tests.dll

# 生成代码覆盖率报告
OpenCover.Console.exe -target:"vstest.console.exe" -targetargs:"WebApplication1.Tests\bin\Release\WebApplication1.Tests.dll" -output:coverage.xml
ReportGenerator.exe -reports:coverage.xml -targetdir:coverage-report
```

### Performance Testing

虽然性能需求（Requirement 7）不在属性测试范围内，但应进行专门的性能测试：

- 使用 Application Insights 监控生产环境响应时间
- 使用负载测试工具（如 Apache JMeter）模拟并发用户
- 设置性能基准并在 CI 中监控性能回归

