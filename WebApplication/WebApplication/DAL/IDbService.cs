using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public interface IDbService
    {
        
        public IEnumerable<Student> getStudents();
        public Student getStudent(string ID);
    }
}