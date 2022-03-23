using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProject_W2022.Models
{
    public class Shift
    {
        [Key]
        public int SHID { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }

    public class ShiftDto
    {
        [Key]
        public int SHID { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }
}