using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Http.Description;
using HospitalProject_W2022.Models;
using System.Web.Script.Serialization;
using System.Web;
using System.Diagnostics;


namespace HospitalProject_W2022.Controllers
{
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
        {
            client = new HttpClient();
            Debug.WriteLine(client);

            client.BaseAddress = new Uri("https://localhost:44377/api/departmentdata/");
            Debug.WriteLine(client.BaseAddress);

        }

        // GET: Department/List
        public ActionResult List()
        {
            //curl https://localhost:44377/api/departmentdata/listdepartments
            string url = "listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("the response code is >>" + response.StatusCode);

            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            Debug.WriteLine("(controller)number of departments received >>");
            Debug.WriteLine(departments.Count());

            return View(departments);
        }

        // GET: Department/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            //curl https://localhost:44377/api/departmentdata/findDepartment/{id}
            string url = "findDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("the response code is >>" + response.StatusCode);

            DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            Debug.WriteLine("departments received >>");
            Debug.WriteLine(selectedDepartment.DName);

            return View(selectedDepartment);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Department/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Department department)
        {
            System.Diagnostics.Debug.WriteLine("the jsonpatlaod is: ");
            // add new department into system using the API
            //curl -H "Content-Type:application/json" -d @department.json https://localhost:44377/api/departmentdata/adddepartment
            string url = "adddepartment";

            string jsonpayload = jss.Serialize(department);
            System.Diagnostics.Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Department/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "findDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(selectedDepartment);
        }

        // POST: Department/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Department department)
        {
            string url = "updatedepartment/" + id;

            string jsonpayload = jss.Serialize(department);
            System.Diagnostics.Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(selectedDepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "deletedepartment/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}