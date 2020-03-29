using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student(studentId: 1, firstName: "Alan", lastName: "Dorien", indexNumber: "s12342"),
                new Student(studentId: 2, firstName: "Marcin", lastName: "Kowal", indexNumber: "s12332"),
                new Student(studentId: 3, firstName: "Lorenz", lastName: "Fatalisya", indexNumber: "s12341")
            };
        }

        public IEnumerable<Student> getStudents()
        {
            return _students;
        }
    }
}