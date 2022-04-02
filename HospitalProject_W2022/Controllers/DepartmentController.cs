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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            Debug.WriteLine(client);

            client.BaseAddress = new Uri("https://localhost:44377/api/departmentdata/");
            Debug.WriteLine(client.BaseAddress);

        }

        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Department/List
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();
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
            GetApplicationCookie();
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
            GetApplicationCookie();

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
            GetApplicationCookie();

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
            GetApplicationCookie();

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
            GetApplicationCookie();

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
            GetApplicationCookie();

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