using System.Collections.Generic;
using API.EntityModels;
using API.Models;

namespace API.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> getStudents();
        public Student getStudent(string ID);
    }
}