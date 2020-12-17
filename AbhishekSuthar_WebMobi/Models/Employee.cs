using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbhishekSuthar_WebMobi.Models
{
    public class Employee
    {
        [Key]

        public int EmpId { get; set; }
        [Required]
        [StringLength(300)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(300)]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }

        public DateTime? Dob { get; set; }

        public string Image { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}