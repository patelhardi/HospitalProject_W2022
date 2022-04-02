using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject_W2022.Models
{
    public class Staff
    {
        [Key]
        public int SID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public long Contact { get; set; }
        public string DID { get; set; }
    }

    public class StaffsDto
    {
        public int SID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public long Contact { get; set; }
        public string DID { get; set; }
    }

}