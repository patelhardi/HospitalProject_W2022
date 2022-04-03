using HospitalProject_W2022.Models;
using HospitalProject_W2022.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProject_W2022.Controllers
{
    public class StaffController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StaffController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/api/StaffData/");
        }

        /// <summary>
        /// Display list of all staffs
        /// select * from staff
        /// </summary>
        /// <returns>List of all staffs</returns>
        // GET: Staff/List
        public ActionResult List()
        {
            string url = "ListStaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffsDto> staffDtos = response.Content.ReadAsAsync<IEnumerable<StaffsDto>>().Result;
            return View(staffDtos);
        }

        /// <summary>
        /// Display list of all staffs as per staff id
        /// select * from staff where staff id = 1
        /// </summary>
        /// <param name="id">staff id passing parameter</param>
        /// <returns>List of all staffs as per staff id</returns>
        // GET: Staff/ListByStaff/5
        public ActionResult ListByStaff(int id)
        {
            string url = "ListStaffsByStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffsDto> staffDtos = response.Content.ReadAsAsync<IEnumerable<StaffsDto>>().Result;
            return View(staffDtos);
        }

        /// <summary>
        /// Display Details of perticular staff
        /// select * from staff where id = 1
        /// </summary>
        /// <param name="id">staff id passing parameter</param>
        /// <returns>Details of selected staff details</returns>
        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            string url = "FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffsDto selectedStaffsDto = response.Content.ReadAsAsync<StaffsDto>().Result;
            return View(selectedStaffsDto);
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>Error page</returns>
        // GET: Staff/Error
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// add staff page
        /// </summary>
        /// <returns>add new staff page</returns>
        // GET: Staff/New
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// create new staff
        /// insert into staff (date, time, reason, pid) values(March 21,2022, 10:30AM, XYZ, 1)
        /// </summary>
        /// <param name="staff"></param>
        /// <returns>added record into staff table</returns>
        // POST: Staff/Create
        [HttpPost]
        public ActionResult Create(Staff staff)
        {
            string url = "AddStaff";

            string jsonpayload = jss.Serialize(staff);

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
        /// Display Edit page for perticular staff
        /// </summary>
        /// <param name="id">staff id</param>
        /// <returns>edit page</returns>
        // GET: Staff/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateStaff viewModel = new UpdateStaff();

            //selected staff information
            string url = "FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffsDto selectedStaffsDto = response.Content.ReadAsAsync<StaffsDto>().Result;
            viewModel.SelectedStaff = selectedStaffsDto;

            //list of staff dropdown 
            url = "ListStaffs";
            response = client.GetAsync(url).Result;
            IEnumerable<StaffsDto> staffOptions = response.Content.ReadAsAsync<IEnumerable<StaffsDto>>().Result;
            viewModel.StaffOptions = staffOptions;

            return View(viewModel);
        }

        /// <summary>
        /// update staff details into database
        /// update staff set date=@date, time=@time, reason=@reason where id=1
        /// </summary>
        /// <param name="id">staff id</param>
        /// <param name="staff">appointmet details such as date, time, reason, staffid</param>
        /// <returns>update information into staff table for perticular staff</returns>
        // POST: Staff/Update/5
        [HttpPost]
        public ActionResult Update(int id, Staff staff)
        {
            string url = "UpdateStaff/" + id;

            string jsonpayload = jss.Serialize(staff);

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
        /// display delete confirmation page for perticular staff
        /// </summary>
        /// <param name="id">staff id</param>
        /// <returns>delete confirm page</returns>
        // GET: Staff/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffsDto selectedStaffsDto = response.Content.ReadAsAsync<StaffsDto>().Result;
            return View(selectedStaffsDto);
        }

        /// <summary>
        /// delete record from database
        /// delete from staff where id = 1
        /// </summary>
        /// <param name="id">staff id</param>
        /// <returns>delete perticular staff record from the staff table </returns>
        // POST: Staff/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteStaff/" + id;
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
