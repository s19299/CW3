using System;

namespace API.Models
{
    public class Student
        {
            public int StudentID { get; set; }
        
            public string firstName { get; set; }
        
            public string lastName { get; set; }
        
            public string indexNumber { get; set; }
        
            public DateTime birthDate { get; set; }
        
            public string course { get; set; }
        
            public int semester { get; set; }
            
            public int IdEnrollment { get; set; }

            public Student(int studentId, string firstName, string lastName, string indexNumber, DateTime birthDate, string course, int semester)
            {
                this.StudentID = studentId;
                this.firstName = firstName;
                this.lastName = lastName;
                this.indexNumber = indexNumber;
                this.birthDate = birthDate;
                this.course = course;
                this.semester = semester;
            }
        
            public Student( string firstName, string lastName, string indexNumber, DateTime birthDate, string course, int semester)
            {
                this.firstName = firstName;
                this.lastName = lastName;
                this.indexNumber = indexNumber;
                this.birthDate = birthDate;
                this.course = course;
                this.semester = semester;
            }
            
            public Student( string firstName, string lastName, string indexNumber, DateTime birthDate, Enrollment enrollment)
            {
                this.firstName = firstName;
                this.lastName = lastName;
                this.indexNumber = indexNumber;
                this.birthDate = birthDate;
                this.course = enrollment.courseName;
                this.semester = enrollment.semester;
                this.IdEnrollment = enrollment.IdEnrollment;
            }

            public Student()
            {
            
            }
    }
}