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
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }

        /// <summary>
        /// Display list of all staffs
        /// select * from staff
        /// </summary>
        /// <returns>List of all staffs</returns>
        // GET: Staff/List
        public ActionResult List()
        {
            string url = "StaffData/ListStaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffDto> staffDtos = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
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
            DetailsStaff viewModel = new DetailsStaff();

            //communicate with datacontroller class
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffDto selectedStaffsDto = response.Content.ReadAsAsync<StaffDto>().Result;

            viewModel.SelectedStaff = selectedStaffsDto;

            url = "ShiftData/ListShiftForStaff/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ShiftDto> KeptShifts = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;

            viewModel.KeptShifts = KeptShifts;

            return View(viewModel);
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
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departmentOptions);
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
            string url = "StaffData/AddStaff";

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
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffDto selectedStaffsDto = response.Content.ReadAsAsync<StaffDto>().Result;
            viewModel.SelectedStaff = selectedStaffsDto;

            //list of staff dropdown 
            url = "DepartmentData/ListDepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            viewModel.DepartmentOptions = departmentOptions;

            url = "ShiftData/ListShiftForStaff/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ShiftDto> ShiftOptions = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;

            viewModel.ShiftOptions = ShiftOptions;

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
            string url = "StaffData/UpdateStaff/" + id;

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
            string url = "StaffData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffDto selectedStaffsDto = response.Content.ReadAsAsync<StaffDto>().Result;
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
            string url = "StaffData/DeleteStaff/" + id;
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
