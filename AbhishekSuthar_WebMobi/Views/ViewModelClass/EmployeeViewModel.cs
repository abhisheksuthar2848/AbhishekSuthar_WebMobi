using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbhishekSuthar_WebMobi.Views.ViewModelClass
{
    public class EmployeeViewModel
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string Image { get; set; }
        public string DepartmentName { get; set; }
    }
}