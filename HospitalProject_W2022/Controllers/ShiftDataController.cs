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

namespace HospitalProject_W2022.Controllers
{
    public class ShiftDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// display list of all shifts
        /// </summary>
        /// <returns>Display list of shifts with date, and type</returns>
        // GET: api/ShiftData/ListShifts
        [HttpGet]
        [Route("api/ShiftData/ListShifts")]
        public IEnumerable<ShiftDto> ListShifts()
        {
            List<Shift> shifts = db.Shifts.OrderBy(s => s.Date).ToList();
            List<ShiftDto> shiftDtos = new List<ShiftDto>();
            shifts.ForEach(s => shiftDtos.Add(new ShiftDto()
            {
                SHID = s.SHID,
                Date = s.Date,
                Type = s.Type,
                FName = s.Staff.FName,
                LName = s.Staff.LName,
                SID = s.SID
            })) ;
            return shiftDtos;
        }

        /// <summary>
        /// display list of all staffs
        /// </summary>
        /// <returns>Display list of staffs with name</returns>
        // GET: api/ShiftData/ListStaffs
        [HttpGet]
        [Route("api/ShiftData/ListStaffs")]
        public IEnumerable<StaffDto> ListStaffs()
        {
            List<Staff> staffs = db.Staffs.OrderBy(s => s.FName).ToList();
            List<StaffDto> staffDtos = new List<StaffDto>();
            staffs.ForEach(s => staffDtos.Add(new StaffDto()
            {
                SID = s.SID,
                FName = s.FName,
                LName = s.LName
            }));
            return staffDtos;
        }

        /// <summary>
        /// display result of perticular shift
        /// </summary>
        /// <param name="id">passing shift id parameter</param>
        /// <returns>details of perticular shift</returns>
        // GET: api/ShiftData/FindShift/5
        [ResponseType(typeof(Shift))]
        [HttpGet]
        [Route("api/ShiftData/FindShift/{id}")]
        public IHttpActionResult FindShift(int id)
        {
            Shift shift = db.Shifts.Find(id);
            ShiftDto shiftDto = new ShiftDto()
            {
                SHID = shift.SHID,
                Date = shift.Date,
                Type = shift.Type,
                FName = shift.Staff.FName,
                LName = shift.Staff.LName,
                SID = shift.SID
            };
            if (shift == null)
            {
                return NotFound();
            }

            return Ok(shiftDto);
        }

        /// <summary>
        /// update record of perticular shift
        /// </summary>
        /// <param name="id">shift id passing parameter</param>
        /// <param name="shift">shift data</param>
        /// <returns>update information into shift table</returns>
        // POST: api/ShiftData/UpdateShift/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateShift(int id, Shift shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shift.SHID)
            {
                return BadRequest();
            }

            db.Entry(shift).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
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
        /// add new shift record into the database
        /// </summary>
        /// <param name="shift">shift data</param>
        /// <returns>add data into shift table</returns>
        // POST: api/ShiftData/AddShift
        [ResponseType(typeof(Shift))]
        [HttpPost]
        public IHttpActionResult AddShift(Shift shift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shifts.Add(shift);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = shift.SHID }, shift);
        }

        /// <summary>
        /// Delete perticular shift 
        /// </summary>
        /// <param name="id">passing shift id parameter</param>
        /// <returns>delete record from the database</returns>
        // POST: api/ShiftData/DeleteShift/5
        [ResponseType(typeof(Shift))]
        [HttpPost]
        public IHttpActionResult DeleteShift(int id)
        {
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return NotFound();
            }

            db.Shifts.Remove(shift);
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

        private bool ShiftExists(int id)
        {
            return db.Shifts.Count(e => e.SHID == id) > 0;
        }
    }
}