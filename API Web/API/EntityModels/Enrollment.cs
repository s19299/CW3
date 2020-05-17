using System;

namespace API.EntityModels
{
    public class Enrollment
    {
        public int IdEnrollment { get; set; }
        
        public int IdStudy { get; set; }
        
        public string courseName { get; set; }
        
        public int semester { get; set; }
        
        public DateTime startDate { get; set; }

        public Enrollment(int idEnrollment, int idStudy, string courseName, int semester)
        {
            this.semester = semester;
            this.courseName = courseName;
            this.IdEnrollment = idEnrollment;
            this.IdStudy = idStudy;
            this.startDate = DateTime.Today;

        }
        
    }
}