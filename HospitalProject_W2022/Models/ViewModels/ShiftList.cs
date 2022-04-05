using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class ShiftList
    {

        public bool IsAdmin { get; set; }

        public IEnumerable<ShiftDto> Shifts { get; set; }
    }
}