using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class DetailsShift
    {
        public ShiftDto SelectedShift { get; set; }
        public IEnumerable<StaffDto> KeptStaff { get; set; }
        public IEnumerable<StaffDto> StaffOptions { get; set; }
    }
}