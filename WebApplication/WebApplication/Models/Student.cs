namespace WebApplication.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        
        public string firstName { get; set; }
        
        public string lastName { get; set; }
        
        public string indexNumber { get; set; }

        public Student(int studentId, string firstName, string lastName, string indexNumber)
        {
            this.StudentID = studentId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.indexNumber = indexNumber;
        }
    }
}