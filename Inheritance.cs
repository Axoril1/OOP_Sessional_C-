using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagement{
    abstract class Person{
        public string Name { get; }
        public string Email { get; }

        protected Person(string Name, string Email){
            this.Name = Name;
            this.Email = Email;
        }

        public virtual void ShowInfo(){
            Console.WriteLine($"Name: {Name}  |  Email: {Email}");
        }
    }

    class Student : Person{
        public string StudentId { get; }
        public List<Enrollment> Enrollments { get; }

        public Student(string StudentId, string Name, string Email)
            : base(Name, Email){
            this.StudentId = StudentId;
            Enrollments = new List<Enrollment>();
        }

        public void EnrollInCourse(Course Course){
            if(Enrollments.Any(e => e.Course.CourseCode == Course.CourseCode)) return;

            Enrollment NewEnrollment = new Enrollment(this, Course);
            Enrollments.Add(NewEnrollment);
            Course.Enrollments.Add(NewEnrollment);
        }

        public override void ShowInfo(){
            Console.WriteLine($"Student: {StudentId}  |  {Name}");
        }

        public void PrintTranscript(){
            Console.WriteLine($"\nTranscript for {StudentId} - {Name}");
            if(!Enrollments.Any()){
                Console.WriteLine("  No courses enrolled.");
                return;
            }
            foreach(Enrollment E in Enrollments){
                string GradeText = E.Grade.HasValue
                    ? E.Grade.Value.ToString("F1")
                    : "N/A";
                Console.WriteLine($"  {E.Course.CourseCode} - {E.Course.Title}  |  Grade: {GradeText}");
            }
        }
    }

    class Teacher : Person{
        public string EmployeeId { get; }
        public List<Course> CoursesTaught { get; }

        public Teacher(string EmployeeId, string Name, string Email)
            : base(Name, Email){
            this.EmployeeId = EmployeeId;
            CoursesTaught = new List<Course>();
        }

        public void AssignCourse(Course Course){
            if(CoursesTaught.Contains(Course)) return;
            CoursesTaught.Add(Course);
            Course.Instructor = this;
        }

        public void AssignGrade(string StudentId, string CourseCode, decimal Grade){
            Enrollment Found = CoursesTaught
                .SelectMany(c => c.Enrollments)
                .FirstOrDefault(e =>
                    e.Student.StudentId == StudentId &&
                    e.Course.CourseCode == CourseCode);

            if(Found != null) Found.Grade = Grade;
        }

        public override void ShowInfo(){
            Console.WriteLine($"Teacher: {EmployeeId}  |  {Name}");
        }

        public void ListCourses(){
            Console.WriteLine($"\nCourses taught by {Name}:");
            if(!CoursesTaught.Any()){
                Console.WriteLine("  None");
                return;
            }
            foreach(Course C in CoursesTaught) Console.WriteLine($"  {C.CourseCode} - {C.Title}");
        }
    }

    class Course{
        public string CourseCode { get; }
        public string Title { get; }
        public Teacher Instructor { get; set; }
        public List<Enrollment> Enrollments { get; }

        public Course(string CourseCode, string Title){
            this.CourseCode = CourseCode;
            this.Title = Title;
            Enrollments = new List<Enrollment>();
        }

        public void ListEnrollments(){
            Console.WriteLine($"\nRoster for {CourseCode} - {Title}");
            if(!Enrollments.Any()){
                Console.WriteLine("  No students enrolled.");
                return;
            }

            foreach(Enrollment E in Enrollments){
                string GradeText = E.Grade.HasValue
                    ? E.Grade.Value.ToString("F1")
                    : "N/A";
                Console.WriteLine($"  {E.Student.StudentId} - {E.Student.Name}  |  Grade: {GradeText}");
            }
        }
    }

    class Enrollment{
        public Student Student { get; }
        public Course Course { get; }
        public decimal? Grade { get; set; }

        public Enrollment(Student Student, Course Course){
            this.Student = Student;
            this.Course = Course;
            this.Grade = null;
        }
    }

    class School{
        public List<Student> Students { get; }
        public List<Teacher> Teachers { get; }
        public List<Course> Courses { get; }

        public School(){
            Students = new List<Student>();
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
        }

        public void RegisterStudent(Student Student){
            if(!Students.Any(s => s.StudentId == Student.StudentId))
                Students.Add(Student);
        }

        public void RegisterTeacher(Teacher Teacher)
        {
            if(!Teachers.Any(t => t.EmployeeId == Teacher.EmployeeId))
                Teachers.Add(Teacher);
        }

        public void AddCourse(Course Course){
            if(!Courses.Any(c => c.CourseCode == Course.CourseCode))
                Courses.Add(Course);
        }

        public void EnrollStudent(string StudentId, string CourseCode){
            Student FoundStudent = Students
                .FirstOrDefault(s => s.StudentId == StudentId);
            Course FoundCourse = Courses
                .FirstOrDefault(c => c.CourseCode == CourseCode);

            if(FoundStudent != null && FoundCourse != null) FoundStudent.EnrollInCourse(FoundCourse);
        }

        public void AssignTeacherToCourse(string EmployeeId, string CourseCode){
            Teacher FoundTeacher = Teachers
                .FirstOrDefault(t => t.EmployeeId == EmployeeId);
            Course FoundCourse = Courses
                .FirstOrDefault(c => c.CourseCode == CourseCode);

            if (FoundTeacher != null && FoundCourse != null)
                FoundTeacher.AssignCourse(FoundCourse);
        }

        public void AssignGrade(string EmployeeId, string StudentId, string CourseCode, decimal Grade){
            Teacher FoundTeacher = Teachers
                .FirstOrDefault(t => t.EmployeeId == EmployeeId);

            if(FoundTeacher != null) FoundTeacher.AssignGrade(StudentId, CourseCode, Grade);
        }

        public void ShowSummary(){
            Console.WriteLine("=== School Summary ===\n");
            Console.WriteLine("Students:");
            foreach(Student S in Students) S.ShowInfo();
            Console.WriteLine("\nTeachers:");
            foreach(Teacher T in Teachers) T.ShowInfo();
            Console.WriteLine("\nCourses:");
            foreach(Course C in Courses) Console.WriteLine($"  {C.CourseCode} - {C.Title}  |  Instructor: {(C.Instructor?.Name ?? "TBA")}");
        }
    }

    class Program{
        static void Main(){
            School MySchool = new School();
            Teacher TeacherA = new Teacher("T100", "Alice Johnson", "alice@school.edu");
            Teacher TeacherB = new Teacher("T101", "Bob Smith", "bob@school.edu");
            MySchool.RegisterTeacher(TeacherA);
            MySchool.RegisterTeacher(TeacherB);
            Course CourseA = new Course("CS101", "Intro to Programming");
            Course CourseB = new Course("MA201", "Discrete Mathematics");
            MySchool.AddCourse(CourseA);
            MySchool.AddCourse(CourseB);
            MySchool.AssignTeacherToCourse("T100", "CS101");
            MySchool.AssignTeacherToCourse("T101", "MA201");
            Student StudentA = new Student("S001", "Carol Lee", "carol@student.edu");
            Student StudentB = new Student("S002", "David Kim", "david@student.edu");
            Student StudentC = new Student("S003", "Eva Chen", "eva@student.edu");
            MySchool.RegisterStudent(StudentA);
            MySchool.RegisterStudent(StudentB);
            MySchool.RegisterStudent(StudentC);
            MySchool.EnrollStudent("S001", "CS101");
            MySchool.EnrollStudent("S001", "MA201");
            MySchool.EnrollStudent("S002", "CS101");
            MySchool.EnrollStudent("S003", "MA201");
            MySchool.AssignGrade("T100", "S001", "CS101", 95m);
            MySchool.AssignGrade("T100", "S002", "CS101", 88m);
            MySchool.AssignGrade("T101", "S001", "MA201", 92m);
            MySchool.AssignGrade("T101", "S003", "MA201", 85m);
            MySchool.ShowSummary();
            TeacherA.ListCourses();
            TeacherB.ListCourses();
            CourseA.ListEnrollments();
            CourseB.ListEnrollments();
            StudentA.PrintTranscript();
            StudentB.PrintTranscript();
            StudentC.PrintTranscript();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
