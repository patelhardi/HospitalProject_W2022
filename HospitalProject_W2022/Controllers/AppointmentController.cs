using HospitalProject_W2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProject_W2022.Controllers
{
    public class AppointmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/api/AppointmentData/");
        }

        /// <summary>
        /// Display list of all appointments
        /// select * from appointment
        /// </summary>
        /// <returns>List of all appointments</returns>
        // GET: Appointment/List
        public ActionResult List()
        {
            string url = "ListAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> appointmentDtos = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            return View(appointmentDtos);
        }

        /// <summary>
        /// Display list of all appointments as per patient id
        /// select * from appointment where patient id = 1
        /// </summary>
        /// <param name="id">patient id passing parameter</param>
        /// <returns>List of all appointments as per patient id</returns>
        // GET: Appointment/ListByPatient/5
        public ActionResult ListByPatient(int id)
        {
            string url = "ListAppointmentsByPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> appointmentDtos = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            return View(appointmentDtos);
        }

        /// <summary>
        /// Display Details of perticular appointment
        /// select * from appointment where id = 1
        /// </summary>
        /// <param name="id">appointmnet id passing parameter</param>
        /// <returns>Details of selected appointment</returns>
        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            string url = "FindAppointment/" + id;
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
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// create new appointment
        /// insert into appointment (date, time, reason, pid) values(March 21,2022, 10:30AM, XYZ, 1)
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>added record into appointment table</returns>
        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment appointment)
        {
            string url = "AddAppointment";

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
        public ActionResult Edit(int id)
        {
            string url = "FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedAppointmentDto = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(selectedAppointmentDto);
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
        public ActionResult Update(int id, Appointment appointment)
        {
            string url = "UpdateAppointment/" + id;

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
            string url = "FindAppointment/" + id;
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
        public ActionResult Delete(int id)
        {
            string url = "DeleteAppointment/" + id;
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
