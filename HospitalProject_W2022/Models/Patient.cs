using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HospitalProject_W2022.Models
{
    public class Patient
    {
        [Key]
        public int PID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string HC { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public long Contact { get; set; }
    }
}