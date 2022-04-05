using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class AppointmentList
    {

        public bool IsAdmin { get; set; }

        public IEnumerable<AppointmentDto> Appointments { get; set; }

    }
}