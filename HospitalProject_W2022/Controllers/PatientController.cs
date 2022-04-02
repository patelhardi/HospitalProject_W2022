using HospitalProject_W2022.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProject_W2022.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            Debug.WriteLine(client);

            client.BaseAddress = new Uri("https://localhost:44377/api/patientdata/");
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

        // GET: Patient/List
        public ActionResult List()
        {
            GetApplicationCookie();
            //curl https://localhost:44377/api/patientdata/listpatients
            string url = "listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("the response code is >>" + response.StatusCode);

            IEnumerable<PatientsDto> patient = response.Content.ReadAsAsync<IEnumerable<PatientsDto>>().Result;
            Debug.WriteLine("(controller)number of patients received >>");

            return View(patient);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            //curl https://localhost:44377/api/patientdata/findPatient/{id}
            string url = "findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("the response code is >>" + response.StatusCode);

            PatientsDto selectedPatient = response.Content.ReadAsAsync<PatientsDto>().Result;
            Debug.WriteLine("patients received >>");
            Debug.WriteLine(selectedPatient.FName);

            return View(selectedPatient);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Patient/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            GetApplicationCookie();

            System.Diagnostics.Debug.WriteLine("the jsonpatlaod is: ");
            // add new patient into system using the API
            //curl -H "Content-Type:application/json" -d @patient.json https://localhost:44377/api/patientdata/addpatient
            string url = "addpatient";

            string jsonpayload = jss.Serialize(patient);
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

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();

            string url = "findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientsDto selectedPatient = response.Content.ReadAsAsync<PatientsDto>().Result;

            return View(selectedPatient);
        }

        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient patient)
        {
            GetApplicationCookie();

            string url = "updatepatient/" + id;

            string jsonpayload = jss.Serialize(patient);
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

        // GET: Patient/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();

            string url = "findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientsDto selectedPatient = response.Content.ReadAsAsync<PatientsDto>().Result;

            return View(selectedPatient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();

            string url = "deletepatient/" + id;

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
