using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class DetailsStaff
    {
        public StaffDto SelectedStaff { get; set; }
        public IEnumerable<ShiftDto> KeptShifts { get; set; }
    }
}