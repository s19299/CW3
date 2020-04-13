using System;

namespace API.Models
{
    public class EnrollResponse
    {
        public int idEnrollment { get; set; }

        public int idStudy { get; set; }
        
        public  int semester { get; set; }
        
        public DateTime start { get; set; }
        
        
    }
}