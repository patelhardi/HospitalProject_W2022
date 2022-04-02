using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models.ViewModels
{
    public class UpdatePatient
    {
        public PatientsDto SelectedPatient { get; set; }
        public IEnumerable<PatientsDto> PatientOptions { get; set; }
    }
}