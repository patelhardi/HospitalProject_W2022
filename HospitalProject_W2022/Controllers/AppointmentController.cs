using HospitalProject_W2022.Models;
using HospitalProject_W2022.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;


namespace HospitalProject_W2022.Controllers
{
    public class AppointmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            Debug.WriteLine(client);

            client.BaseAddress = new Uri("https://localhost:44377/api/");
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


        /// <summary>
        /// Display list of all appointments
        /// select * from appointment
        /// </summary>
        /// <returns>List of all appointments</returns>
        // GET: Appointment/List

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();

            AppointmentList ViewModel = new AppointmentList();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

            string url = "AppointmentData/ListAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            ViewModel.Appointments = appointments;
            return View(ViewModel);
        }

        /// <summary>
        /// Display Details of perticular appointment
        /// select * from appointment where id = 1
        /// </summary>
        /// <param name="id">appointment id passing parameter</param>
        /// <returns>Details of selected appointment details</returns>
        // GET: Appointment/Details/5
        [Authorize(Roles = "Admin,Patient")]

        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedAppointmentDto = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(selectedAppointmentDto);
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>Error page</returns>
        // GET: Appointment/Error
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// add appointment page
        /// </summary>
        /// <returns>add new appointment page</returns>
        // GET: Appointment/New
        [Authorize(Roles = "Patient")]
        public ActionResult New()
        {
            GetApplicationCookie();
            //need patient information for patient dropdown
            string url = "PatientData/ListPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> patientOptions = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            return View(patientOptions);
        }

        /// <summary>
        /// create new appointment
        /// insert into appointment (date, time, reason, pid) values(March 21,2022, 10:30AM, XYZ, 1)
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>added record into appointment table</returns>
        // POST: Appointment/Create
        [HttpPost]
        [Authorize(Roles = "Patient")]

        public ActionResult Create(Appointment appointment)
        {
            string url = "AppointmentData/AddAppointment";

            string jsonpayload = jss.Serialize(appointment);

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

        /// <summary>
        /// Display Edit page for perticular appointment
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns>edit page</returns>
        // GET: Appointment/Edit/5
        [Authorize(Roles = "Patient")]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            UpdateAppointment viewModel = new UpdateAppointment();

            //selected appointment information
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto selectedAppointmentDto = response.Content.ReadAsAsync<AppointmentDto>().Result;
            viewModel.SelectedAppointment = selectedAppointmentDto;

            //list of staff dropdown 
            url = "PatientData/ListPatients";
            response = client.GetAsync(url).Result;
            IEnumerable<PatientDto> patientOptions = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            viewModel.PatientOptions = patientOptions;

            return View(viewModel);
        }

        /// <summary>
        /// update appointment details into database
        /// update appointment set date=@date, time=@time, reason=@reason where id=1
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <param name="appointment">appointmet details such as date, time, reason, patientid</param>
        /// <returns>update information into appointment table for perticular appointment</returns>
        // POST: Appointment/Update/5
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public ActionResult Update(int id, Appointment appointment)
        {
            GetApplicationCookie();
            string url = "AppointmentData/UpdateAppointment/" + id;

            string jsonpayload = jss.Serialize(appointment);

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

        /// <summary>
        /// display delete confirmation page for perticular appointment
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns>delete confirm page</returns>
        // GET: Appointment/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedAppointmentDto = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(selectedAppointmentDto);
        }

        /// <summary>
        /// delete record from database
        /// delete from appointment where id = 1
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns>delete perticular appointment record from the appointment table </returns>
        // POST: Appointment/Delete/5
        [HttpPost]
        [Authorize(Roles = "Patient")]

        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "AppointmentData/DeleteAppointment/" + id;
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
