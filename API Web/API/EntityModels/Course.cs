using System.Collections.Generic;

namespace API.EntityModels
{
    public class Course
    {
        public Course()
        {
            Enrollment = new HashSet<Enrollment>();
        }

        public int IdStudy { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Enrollment> Enrollment { get; set; }
    }
    }
