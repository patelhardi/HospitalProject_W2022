using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject_W2022.Models
{
    public class Department
    {
        [Key]
        public int DID { get; set; }
        public string DName { get; set; }
    }

    // transfer data
    public class DepartmentDto
    {
        public int DID { get; set; }
        public string DName { get; set; }
    }
}