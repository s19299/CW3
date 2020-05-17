using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using API.EntityModels;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class SQLserverDbService : IStudentDbService
    {
        public SQLserverDbService()
        {

        }

        public IEnumerable<Student> getStudents()
        {

            List<Student> students = new List<Student>();

            using (var connection =
                new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var command = new SqlCommand())
            {

                command.Connection = connection;
                command.CommandText =
                    "select * from Student join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment join Studies on Enrollment.IdStudy=Studies.IdStudy";
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var student = new Student(reader["firstName"].ToString(), reader["lastName"].ToString(),
                        reader["indexNumber"].ToString(),
                        DateTime.Parse(reader["birthDate"].ToString()), reader["course"].ToString(),
                        int.Parse(reader["semester"].ToString()));

                    students.Add(student);
                }
            }

            return students;
        }


        public Student getStudent(string indexNum)
        {
            using (var connection =
                new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    "select * from Student join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment join Studies on Enrollment.IdStudy = Studies.IdStudy where indexNumber = @indexNum";
                command.Parameters.AddWithValue("indexNumber", indexNum);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var student = new Student(reader["firstName"].ToString(), reader["lastName"].ToString(),
                        reader["indexNumber"].ToString(),
                        DateTime.Parse(reader["birthDate"].ToString()), reader["course"].ToString(),
                        int.Parse(reader["semester"].ToString()));

                    return student;
                }

                else
                    return null;
            }
        }
        

        public EnrollResponse enrollStudent(EnrollRequest requestStudent)
        {
            using (var connection =
                new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                DateTime startDate = DateTime.Today;

                command.Connection = connection;
                command.CommandText = "select * from Studies where Name = @name";
                command.Parameters.AddWithValue("name", requestStudent.course);
                connection.Open();

                var reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    throw new Exception("nie znaleziono studii: " + requestStudent.course);
                }


                int idStudies = (int) reader["IdStudy"];
                int idEnrollment = 0;

                if (this.getStudent(requestStudent.indexNumber) != null)
                {
                    throw new Exception("mamy studenta o podanym indeksie");
                }

                reader.Close();

                command.CommandText =
                    "select IdStudy from Studies join Enrollment on Studies.IdStudy = Enrollment.IdStudy where Semester = 1 and IdStudy = @IdStudy";
                command.Parameters.AddWithValue("IdStudy", idStudies);

                reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();
                    command.CommandText = "select count(IdEnrollment) as maxEnroll from ENrollment";
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        idEnrollment = (int) reader["maxEnroll"];
                    }
                    else
                    {
                        idEnrollment = 1;
                    }

                    reader.Close();
                    command.CommandText = "insert into Enrollment values(@idEnrollment, 1, @idStudy, @startDate)";
                    command.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    command.Parameters.AddWithValue("idStudy", idStudies);
                    command.Parameters.AddWithValue("startDate", DateTime.Today);
                }
                else
                {
                    startDate = (DateTime) reader["startDate"];
                    idEnrollment = (int) reader["IdEnrollment"];
                    reader.Close();
                }

                //dodaje studenta
                command.CommandText =
                    "insert into Student values (@indexNumber, @firstName, @lastName, @birthDate, IdEnrollment)";
                command.Parameters.AddWithValue("indexNumber", requestStudent.indexNumber);
                command.Parameters.AddWithValue("firstName", requestStudent.firstName);
                command.Parameters.AddWithValue("lastName", requestStudent.lastName);
                command.Parameters.AddWithValue("birthDate", requestStudent.birthDate);
                command.Parameters.AddWithValue("IdEnrollment", idEnrollment);

                command.ExecuteNonQuery();
                return new EnrollResponse("Success!")
                    {idEnrollment = idEnrollment, semester = 1, idStudy = idStudies, start = startDate};

            }


        }

        public void UpdateStudent(UpdateRequest updaterequest)
        {
            var db = new s19299Context();
            var student = db.Students.First(student1 => student1.StudentID == updaterequest.StudentID);
            student.firstName = updaterequest.firstName;
            student.lastName = updaterequest.lastName;
            student.birthDate = updaterequest.birthDate;
            student.IdEnrollment = updaterequest.enrollment;
            db.SaveChanges();
        }

        public EnrollResponse promoteStudents(EnrollRequest enrollRequest)
        {
            {
                var db = new s19299Context();
                var studies = db.Courses.First(stud => stud.Name.Equals(enrollRequest.course));
                var student = db.Students.First(s => s.indexNumber == enrollRequest.indexNumber);

                if (studies.Equals(null))
                {
                    throw new Exception("No course as: " + enrollRequest.course);
                }


                var enrollment = db.Enrollment.First(enrollement =>
                    enrollement.IdStudy == studies.IdStudy && enrollement.semester == 1);
                var newStudent = new Student
                {
                    indexNumber = enrollRequest.indexNumber,
                    firstName = enrollRequest.firstName,
                    lastName = enrollRequest.lastName,
                    birthDate = enrollRequest.birthDate
                };


                if (enrollment.Equals(null))
                {
                    throw new Exception("Enrollment as such doesn't exist");
                }
                else

                {
                    newStudent.course = enrollRequest.course;
                    newStudent.indexNumber = enrollRequest.indexNumber;
                    newStudent.enrollment = enrollment;
                    newStudent.firstName = enrollRequest.firstName;
                    newStudent.lastName = enrollRequest.lastName;
                    newStudent.birthDate = enrollRequest.birthDate;
                }

                db.Add(newStudent);
                db.SaveChanges();
                
                return new EnrollResponse("Success!")
                {
                    idEnrollment = enrollment.IdEnrollment,
                    idStudy = enrollment.IdStudy,
                    semester = enrollment.semester,
                    start = enrollment.startDate
                };

            }
        }
    }
}
