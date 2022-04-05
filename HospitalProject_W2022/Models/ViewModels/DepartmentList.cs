using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class DepartmentList
    {

        public bool IsAdmin { get; set; }

        //public DepartmentDto Departments { get; set; }//
        public IEnumerable<DepartmentDto> Departments { get; set; }

    }
}