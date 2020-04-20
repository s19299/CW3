using System.Collections.Generic;
using API.Models;

namespace API.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> getStudents();
        public Student getStudent(string ID);
    }
}