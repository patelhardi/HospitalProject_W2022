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
        [ForeignKey("Department")]
        public int DID { get; set; }
        public virtual Department Department { get; set; }

        //many shifts for one staff
        public ICollection<Shift> shifts { get; set; }
    }

    public class StaffDto
    {
        public int SID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public long Contact { get; set; }
        public string DName { get; set; }
        public int DID { get; set; }
    }

}