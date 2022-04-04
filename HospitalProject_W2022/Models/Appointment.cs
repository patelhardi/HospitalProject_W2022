using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_W2022.Models
{
    public class Appointment
    {
        [Key]
        public int AID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Reason { get; set; }
        [ForeignKey("Patient")]
        public int PID { get; set; }
        public virtual Patient Patient { get; set; }
    }

    public class AppointmentDto
    {
        public int AID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Reason { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int PID { get; set; }
    }
}