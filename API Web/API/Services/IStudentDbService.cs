using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using API.Models;

namespace API.Services
{
    public interface IStudentDbService
    {
        EnrollResponse enrollStudent(EnrollRequest request);

        EnrollResponse promoteStudents(string course, int semester);
        
        public IEnumerable<Student> getStudents();
        public Student getStudent(string ID);
    }
}