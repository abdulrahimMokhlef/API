using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
     
        [StringLength(15),MinLength(2,ErrorMessage ="Name Must be more than one letter"),MaxLength(15, ErrorMessage ="Name Must be less than one letter")]
        public string? DeptName { get; set; }

        [RegularExpression("[a-zA-Z]{3,10}",ErrorMessage ="Manger Name Must be Only Letters from 3 to 10 Letters")]  
        public string? MgrName { get; set; } 
        public string? Location { get; set; }

        //one-to-many relationship
        public ICollection<Student> Students { get; set; }



    }
}
