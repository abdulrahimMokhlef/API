using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace API_Project.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "min length is 3"), MaxLength(20, ErrorMessage = "max length is 20")]
        public string? Name { get; set; }

        [Range(18, 60, ErrorMessage = "Age between 18-60")]
        public int? Age { get; set; }
        
        [RegularExpression("[a-zA-Z]{3,20}", ErrorMessage = "Must be letters only and less than 21")]
        public string? Address { get; set; }

        public string? Image { get; set; }

        // Foreign Key
        public int DeptId { get; set; }
        [ForeignKey("DeptId")]

        [JsonIgnore] 

        public Department? Department { get; set; }

    }
}
