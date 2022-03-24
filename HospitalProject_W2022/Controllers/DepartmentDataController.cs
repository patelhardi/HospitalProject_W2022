using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using HospitalProject_W2022.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;



namespace HospitalProject_W2022.Controllers
{
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentData/ListDepartments
        [HttpGet]
        [Route("api/DepartmentData/ListDepartments")]

        public IEnumerable<DepartmentDto> ListDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()
            {
                DID = d.DID,
                DName = d.DName
            }));
            return DepartmentDtos;

        }

        // GET: api/DepartmentData/FindDepartment/5
        [ResponseType(typeof(Department))]
        [HttpGet]
        [Route("api/DepartmentData/FindDepartment/{id}")]

        public IHttpActionResult FindDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            DepartmentDto DepartmentDto = new DepartmentDto()
            {
                DID = Department.DID,
                DName = Department.DName
            };

            if (Department == null)
            {
                return NotFound();
            }

            return Ok(DepartmentDto);
        }

        // PUT: api/DepartmentData/UpdateDepartment/5
        //Update function
        [ResponseType(typeof(void))]
        [HttpPost]

        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            Debug.WriteLine("debug>>>reached update department method.");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("debug>>>model state is invalid");

                return BadRequest(ModelState);
            }

            if (id != department.DID)
            {
                Debug.WriteLine("debug>>>id mismatch!");
                Debug.WriteLine("Get parameter" + id);
                Debug.WriteLine("Post parameter" + department.DID);



                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    Debug.WriteLine("debug>>>department not found.");

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("debug>>>none of the conditions trigger.");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DepartmentData/AddDepartment
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DID }, department);
        }

        // POST: api/DepartmentData/DeleteDepartment/5
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
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

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DID == id) > 0;
        }
    }
}