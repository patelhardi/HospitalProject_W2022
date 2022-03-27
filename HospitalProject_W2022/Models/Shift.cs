using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        //connect with staff table
        [ForeignKey("Staff")]
        public int SID { get; set; }
        public virtual Staff Staff { get; set; }
    }

    public class ShiftDto
    {
        [Key]
        public int SHID { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int SID { get; set; }
    }

    public class StaffDto
    {
        public int SID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

    }
}