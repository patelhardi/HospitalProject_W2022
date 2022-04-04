using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class UpdateStaff
    {
        public StaffDto SelectedStaff { get; set; }
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
        public IEnumerable<ShiftDto> ShiftOptions { get; set; }
    }
}