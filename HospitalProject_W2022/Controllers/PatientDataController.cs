using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalProject_W2022.Models;

namespace HospitalProject_W2022.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientData
        [HttpGet]
        public IEnumerable<PatientsDto> ListPatients()
        {
            List<Patient> patients = db.Patients.OrderBy(p => p.PID).ToList();
            List<PatientsDto> patientDtos = new List<PatientsDto>();

            patients.ForEach(p => patientDtos.Add(new PatientsDto()
            {
                PID = p.PID,
                FName = p.FName,
                LName = p.LName,
                HC = p.HC,
                DOB = p.DOB,
                Address = p.Address,
                Contact = p.Contact
            }));
            return patientDtos;
        }

        // GET: api/PatientData/5
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientsDto PatientsDto = new PatientsDto()
            {
                PID = Patient.PID,
                FName = Patient.FName,
                LName = Patient.LName,
                HC = Patient.HC,
                DOB = Patient.DOB,
                Address = Patient.Address,
                Contact = Patient.Contact
            };

            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientsDto);
        }

        // PUT: api/PatientData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PID)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/PatientData
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PID }, patient);
        }

        // DELETE: api/PatientData/5
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
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

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PID == id) > 0;
        }
    }
}