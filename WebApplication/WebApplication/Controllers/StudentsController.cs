

using System;
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