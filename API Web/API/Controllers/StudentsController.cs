﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using API.DAL;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("api/students")]
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
                var sqlReader = com.ExecuteReader();
                while (sqlReader.Read())
                {
                    var st = new Student();
                    st.firstName = sqlReader["FirstName"].ToString();
                    return Ok(st.firstName);
                }
            }

            return Ok("powodzenie");
        }

        [HttpGet("{index}")]
        public IActionResult getStudentById(string index)
        {

            List<Student> students = new List<Student>();

            using (var client =
                new SqlConnection("Data Source=db-mssql;Initial Catalog=s19299;Integrated Security=True"))
            using (var com = new SqlCommand())
            {

                com.Connection = client;
                com.CommandText =
                    "select * from Student join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment where indexNumber = @index";
                com.Parameters.AddWithValue("index", index);

                client.Open();
                var sqlReader = com.ExecuteReader();

                while (sqlReader.Read())
                {
                    {
                        var student = new Student(sqlReader["FirstName"].ToString(), sqlReader["LastName"].ToString(),
                            sqlReader["IndexNumber"].ToString(),
                            DateTime.Parse(sqlReader["BirthDate"].ToString()),
                            sqlReader["Name"].ToString(),
                            int.Parse(sqlReader["Semester"].ToString()));

                        students.Add(student);
                    }
                }
            }

            return Ok(students);
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

        public IActionResult login(LoginRequest logRequest)
        {
           // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretPassword"]));
            
            return Ok();
        }

    }
}