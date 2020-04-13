using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class EnrollRequest
    {
        [RegularExpression("s[0-9]+$")]
        public string indexNumber { get; set; }
        
        [Required(ErrorMessage = "Nalezy podac imie")]
        [MaxLength(20)]
        public string firstName { get; set; }
        
        [Required(ErrorMessage = "Nalezy podac nazwisko")]
        [MaxLength(50)]
        public string lastName { get; set; }
        
        [Required(ErrorMessage = "Nalezy podac date urodzenia")]
        [MaxLength(10)]
        public DateTime birthDate { get; set; }
        
        [Required(ErrorMessage = "Nalezy podac kierunek studiow")]
        public string course { get; set; }
    }
}