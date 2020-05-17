using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using API.EntityModels;
using API.Models;

namespace API.Services
{
    public interface IStudentDbService
    {
        EnrollResponse enrollStudent(EnrollRequest request);
        
        public IEnumerable<Student> getStudents();
        public Student getStudent(string ID);
    }
}