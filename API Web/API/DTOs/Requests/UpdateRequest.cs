using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UpdateRequest
    {
        [Required]
        public int StudentID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime birthDate { get; set; }
        public int enrollment { get; set; }
    }
}