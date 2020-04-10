using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using API.Models;

namespace API.DAL
{
    public class MockDbService : IDbService
    {
         private static IEnumerable<Student> _students;


        static MockDbService()
        {
            _students = new List<Student>();
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
                        DateTime.Parse(reader["birthDate"].ToString()).ToShortDateString(), reader["course"].ToString(),
                        int.Parse(reader["semester"].ToString()));

                    students.Add(student);
                }
            }

            return students;
        }


        public Student getStudent(string indexNum)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select * from Student join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment join Studies on Enrollment.IdStudy = Studies.IdStudy where indexNumber = @indexNum";
                command.Parameters.AddWithValue("indexNumber", indexNum);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var student = new Student(reader["firstName"].ToString(), reader["lastName"].ToString(),
                        reader["indexNumber"].ToString(),
                        DateTime.Parse(reader["birthDate"].ToString()).ToShortDateString(), reader["course"].ToString(),
                        int.Parse(reader["semester"].ToString()));

                    return student;
                }
                
                else
                    return null;
            }
        }
        
    }
}