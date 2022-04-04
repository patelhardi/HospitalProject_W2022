using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalProject_W2022.Models;
using System.Diagnostics;

namespace HospitalProject_W2022.Controllers
{
    public class StaffDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Display List of all staffs
        /// </summary>
        /// <returns>List of staffs with department first and last name, date, time, and reason</returns>
        // GET: api/StaffData/ListStaffs
        [HttpGet]
        [Route("api/StaffData/ListStaffs")]
        public IEnumerable<StaffDto> ListStaffs()
        {
            List<Staff> staffs = db.Staffs.OrderBy(s => s.SID).ToList();
            List<StaffDto> staffDtos = new List<StaffDto>();
            staffs.ForEach(s => staffDtos.Add(new StaffDto()
            {
                SID = s.SID,
                FName = s.FName,
                LName = s.LName,
                DOB = s.DOB,
                Contact = s.Contact,
                DName = s.Department.DName,
                DID = s.DID
            }));
            return staffDtos;
        }

        /// <summary>
        /// Display List of all staff for perticular department
        /// </summary>
        /// <param name="id">passing department id parameter</param>
        /// <returns>List of staffs with department name for perticular department</returns>
        // GET: api/StaffData/ListStaffsByDepartment/5
        [HttpGet]
        [Route("api/StaffData/ListStaffsByDepartment/{id}")]
        public IEnumerable<StaffDto> ListStaffsByDepartment(int id)
        {
            List<Staff> staffs = db.Staffs.Where(
                s => s.Department.DID == id).ToList();
            List<StaffDto> staffDtos = new List<StaffDto>();
            staffs.ForEach(s => staffDtos.Add(new StaffDto()
            {
                SID = s.SID,
                FName = s.FName,
                LName = s.LName,
                DOB = s.DOB,
                Contact = s.Contact,
                DName = s.Department.DName,
                DID = s.DID
            }));
            return staffDtos;
        }

        /// <summary>
        /// display list of staffs in perticular shift
        /// </summary>
        /// <param name="id">passing parameter shift id</param>
        /// <returns>list of staffs</returns>
        // GET: api/StaffData/ListStaffsForShift/1
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        public IHttpActionResult ListStaffsForShift(int id)
        {
            List<Staff> Staffs = db.Staffs.Where(
                s => s.shifts.Any(
                    sh => sh.SHID == id
            )).ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();

            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                SID = s.SID,
                FName = s.FName,
                LName = s.LName,
                DOB = s.DOB,
                Contact = s.Contact,
                DID = s.DID
            }));
            return Ok(StaffDtos);
        }

        /// <summary>
        /// display list of staffs that are not in perticular shift
        /// </summary>
        /// <param name="id">passing parameter shift id</param>
        /// <returns>list of staffs</returns>
        // GET: api/StaffData/ListStaffsNotInPerticularShift/1
        [HttpGet]
        public IHttpActionResult ListStaffsNotInPerticularShift(int id)
        {
            List<Staff> Staffs = db.Staffs.Where(
                s => !s.shifts.Any(
                    sh => sh.SHID == id
            )).OrderBy(s => s.FName).ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();
            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                SID = s.SID,
                FName = s.FName,
                LName = s.LName,
                DOB = s.DOB,
                Contact = s.Contact,
                DID = s.DID
            }));
            return Ok(StaffDtos); ;
        }

        /// <summary>
        /// display result of perticular staff
        /// </summary>
        /// <param name="id">passing staff id parameter</param>
        /// <returns>details of perticular staff</returns>
        // GET: api/StaffData/FindStaff/5
        [ResponseType(typeof(Staff))]
        [HttpGet]
        [Route("api/StaffData/FindStaff/{id}")]
        public IHttpActionResult FindStaff(int id)
        {
            //Debug.WriteLine("id:" + id);
            Staff staff = db.Staffs.Find(id);
            //Debug.WriteLine(staff);
            StaffDto staffDto = new StaffDto()
            {

                SID = staff.SID,
                FName = staff.FName,
                LName = staff.LName,
                DOB = staff.DOB,
                Contact = staff.Contact,
                DName = staff.Department.DName,
                DID = staff.DID
            };
            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staffDto);
        }

        /// <summary>
        /// update record of perticular staff
        /// </summary>
        /// <param name="id">staff id passing parameter</param>
        /// <param name="staff">staff data</param>
        /// <returns>update information into appoinment table</returns>
        // POST: api/StaffData/UpdateStaff/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStaff(int id, Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staff.SID)
            {
                return BadRequest();
            }

            db.Entry(staff).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// add new appoitment record into the database
        /// </summary>
        /// <param name="staff">staff data</param>
        /// <returns>add data into staff table</returns>
        // POST: api/StaffData/AddStaff
        [ResponseType(typeof(Staff))]
        [HttpPost]
        public IHttpActionResult AddStaff(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Staffs.Add(staff);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = staff.SID }, staff);
        }

        /// <summary>
        /// Delete perticular staff 
        /// </summary>
        /// <param name="id">passing staff id parameter</param>
        /// <returns>delete record from the database</returns>
        // POST: api/StaffData/DeleteStaff/5
        [ResponseType(typeof(Staff))]
        [HttpPost]
        public IHttpActionResult DeleteStaff(int id)
        {
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            db.Staffs.Remove(staff);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StaffExists(int id)
        {
            return db.Staffs.Count(e => e.SID == id) > 0;
        }
    }
}