using System.Data.SqlClient;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/enrollements")]
    [ApiController]
    
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult enrollStudent(EnrollRequest requestStudent)
        {Enrollment enrollment = null;  

            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var command = new SqlCommand())
            {

                
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    // czy studia istnieja?
                    command.CommandText = "select IdStudy from Studies where Name = @course";
                    command.Parameters.AddWithValue("course", requestStudent.course);

                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        return NoContent();
                    }
                    

                    int idStudies = (int) reader["IdStudy"];
                    int idEnrollment = (int) reader["IdEnrollment"];

                    
                    command.CommandText =
                        "select IdStudy from Studies join Enrollment on Studies.IdStudy = Enrollment.IdStudy where Semester = 1 and Name = @course";
                    command.Parameters.AddWithValue("course", requestStudent.course);
                    
                    enrollment = new Enrollment(idEnrollment, idStudies, requestStudent.course, 1);
                    


                    //dodaje studenta
                    command.CommandText =
                        "insert into Student values (@indexNumber, @firstName, @lastName, @birthDate)";
                    command.Parameters.AddWithValue("indexNumber", requestStudent.indexNumber);
                    command.Parameters.AddWithValue("firstName", requestStudent.firstName);
                    command.Parameters.AddWithValue("lastName", requestStudent.lastName);
                    command.Parameters.AddWithValue("birthDate", requestStudent.birthDate);
                    command.CommandText =
                        "select * from Student join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment";
                    
                    

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException exc)
                {
                    transaction.Rollback();
                }
            }
            return Ok(enrollment);
        }
    }
}