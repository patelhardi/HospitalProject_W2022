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
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Display List of all appointments
        /// </summary>
        /// <returns>List of appointments with patient first and last name, date, time, and reason</returns>
        // GET: api/AppointmentData/ListAppointments
        [HttpGet]
        [Route("api/AppointmentData/ListAppointments")]
        public IEnumerable<AppointmentDto> ListAppointments()
        {
            List<Appointment> appointments = db.Appointments.OrderBy(a => a.Date).ToList();
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();
            appointments.ForEach(a => appointmentDtos.Add(new AppointmentDto()
            {
                AID = a.AID,
                Date = a.Date,
                Time = a.Time,
                Reason = a.Reason,
                FName = a.Patient.FName,
                LName = a.Patient.LName,
                PID = a.PID
            }));
            return appointmentDtos;
        }

        /// <summary>
        /// Display List of all appointments for perticular patient
        /// </summary>
        /// <param name="id">passing patient id parameter</param>
        /// <returns>List of appointments with patient first and last name, date, time, and reason for perticular patient</returns>
        // GET: api/AppointmentData/ListAppointmentsByPatient/5
        [HttpGet]
        [Route("api/AppointmentData/ListAppointmentsByPatient/{id}")]
        public IEnumerable<AppointmentDto> ListAppointmentsByPatient(int id)
        {
            List<Appointment> appointments = db.Appointments.Where(
                a => a.Patient.PID == id).OrderBy(a => a.Date).ToList();
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();
            appointments.ForEach(a => appointmentDtos.Add(new AppointmentDto()
            {
                AID = a.AID,
                Date = a.Date,
                Time = a.Time,
                Reason = a.Reason,
                FName = a.Patient.FName,
                LName = a.Patient.LName,
                PID = a.PID
            }));
            return appointmentDtos;
        }

        /// <summary>
        /// display result of perticular appointment
        /// </summary>
        /// <param name="id">passing appointment id parameter</param>
        /// <returns>details of perticular appointment</returns>
        // GET: api/AppointmentData/FindAppointment/5
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        [Route("api/AppointmentData/FindAppointment/{id}")]
        public IHttpActionResult FindAppointment(int id)
        {
            Debug.WriteLine("id:" + id);
            Appointment appointment = db.Appointments.Find(id);
            Debug.WriteLine(appointment);
            AppointmentDto appointmentDto = new AppointmentDto()
            {
                AID = appointment.AID,
                Date = appointment.Date,
                Time = appointment.Time,
                Reason = appointment.Reason,
                FName = appointment.Patient.FName,
                LName = appointment.Patient.LName,
                PID = appointment.PID
            };
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointmentDto);
        }

        /// <summary>
        /// update record of perticular appointment
        /// </summary>
        /// <param name="id">appointment id passing parameter</param>
        /// <param name="appointment">appointment data</param>
        /// <returns>update information into appoinment table</returns>
        // POST: api/AppointmentData/UpdateAppointment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AID)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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
        /// <param name="appointment">appointment data</param>
        /// <returns>add data into appointment table</returns>
        // POST: api/AppointmentData/AddAppointment
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AID }, appointment);
        }

        /// <summary>
        /// Delete perticular appointment 
        /// </summary>
        /// <param name="id">passing appointment id parameter</param>
        /// <returns>delete record from the database</returns>
        // POST: api/AppointmentData/DeleteAppointment/5
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
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

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AID == id) > 0;
        }
    }
}