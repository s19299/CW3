

using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService idbService;


        public StudentsController(IDbService idbService)
        {
            this.idbService = idbService;
        }

        [HttpGet]
        public IActionResult getStudents()
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var com = new SqlCommand())
            {

                com.Connection = client;
                com.CommandText = "select * Students .....";

                client.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.firstName = dr["FirstName"].ToString();
                    return Ok(st.firstName);
                }
            }

            return Ok("powodzenie");
        }

        [HttpGet("{ID}")]
        public IActionResult getStudentById(string id)
        {
            if (idbService.getStudent(id) != null)
            {
                return Ok(idbService.getStudent(id));
            }

            else
                return NotFound("nie ma studenta o tym id");


        }


        [HttpPost]
        public IActionResult createStudent(Student student)
        {
            student.indexNumber = $"s{new Random().Next(3, 20000)}";
            return Ok(student);
        }

        [HttpPut("{ID}")]
        public IActionResult modifyStudent(int ID)
        {

            if (idbService.getStudents().ToList().Find(i => i.StudentID == ID) != null)
            {
                return Ok("Aktualizacja zakończona");
            }
            else
                return NotFound("Nie ma takiego studenta");
        }

        [HttpDelete("{ID}")]
        public IActionResult deleteStudent(int ID)
        {
            if (idbService.getStudents().ToList().Find(i => i.StudentID == ID) == null)
            {
                return NotFound("Nie ma takiego studenta");
            }
            else
            {
                idbService.getStudents().ToList().RemoveAt(idbService.getStudents().ToList()
                    .IndexOf((idbService.getStudents().ToList().Find(i => i.StudentID == ID))));
            }

            return Ok("Aktualizacja zakonczona");
        }
        
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(idbService.getStudents());
        }

    }
}