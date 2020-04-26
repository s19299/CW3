using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                    throw new Exception("nie znaleziono studi: " + requestStudent.course);
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
                return new EnrollResponse()
                    {idEnrollment = idEnrollment, semester = 1, idStudy = idStudies, start = startDate};

            }


        }
        
        public EnrollResponse promoteStudents(string course, int semester)
        {
            throw new NotImplementedException();
        }
    }
}