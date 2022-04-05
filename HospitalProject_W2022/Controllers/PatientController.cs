using HospitalProject_W2022.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HospitalProject_W2022.Models.ViewModels;

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
            //Debug.WriteLine(client);

            client.BaseAddress = new Uri("https://localhost:44377/api/");
            //Debug.WriteLine(client.BaseAddress);

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
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();
            //curl https://localhost:44377/api/patientdata/listpatients


            PatientList ViewModel = new PatientList();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;


            string url = "patientdata/ListPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("the response code is >>" + response.StatusCode);

            IEnumerable<PatientDto> patient = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            //Debug.WriteLine("(controller)number of patients received >>");
            ViewModel.Patients = patient;

            return View(ViewModel);
        }

        // GET: Patient/Details/5
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int id)
        {
            GetApplicationCookie();

            DetailsPatient ViewModel = new DetailsPatient();
            //curl https://localhost:44377/api/patientdata/findPatient/{id}
            string url = "patientdata/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;


            //Debug.WriteLine("the response code is >>" + response.StatusCode);

            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            ViewModel.SelectedPatient = selectedPatient;
            //Debug.WriteLine("patients received >>");
            //Debug.WriteLine(selectedPatient.FName);

            url = "AppointmentData/ListAppointmentsByPatient/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AppointmentDto> relatedAppointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            ViewModel.RelatedAppointments = relatedAppointments;
            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Patient/New
        [Authorize(Roles = "Admin")]

        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(Patient patient)
        {
            GetApplicationCookie();

            //System.Diagnostics.Debug.WriteLine("the jsonpatlaod is: ");
            // add new patient into system using the API
            //curl -H "Content-Type:application/json" -d @patient.json https://localhost:44377/api/patientdata/addpatient
            string url = "patientdata/AddPatient";

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
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            GetApplicationCookie();

            string url = "patientdata/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(selectedPatient);
        }

        // POST: Patient/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Update(int id, Patient patient)
        {
            GetApplicationCookie();

            string url = "patientdata/UpdatePatient/" + id;

            string jsonpayload = jss.Serialize(patient);
            //System.Diagnostics.Debug.WriteLine(jsonpayload);

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

        // GET: Patient/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();

            string url = "patientdata/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto selectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(selectedPatient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            GetApplicationCookie();

            string url = "patientdata/DeletePatient/" + id;

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
