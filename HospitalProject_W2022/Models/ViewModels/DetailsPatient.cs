using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class DetailsPatient
    {
        public bool IsAdmin { get; set; }
        public PatientDto SelectedPatient { get; set; }
        public IEnumerable<AppointmentDto> RelatedAppointments { get; set; }
    }
}