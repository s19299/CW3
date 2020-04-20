using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class PromoteRequest
    {
        [Required]
        public string course { get; set; }
        
        [Required]
        public int semester { get; set; }
    }
}