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
    public class StaffController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StaffController()
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
        /// <summary>
        /// Display list of all staffs
        /// select * from staff
        /// </summary>
        /// <returns>List of all staffs</returns>
        // GET: Staff/List
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();

            StaffList ViewModel = new StaffList();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;


            string url = "StaffData/ListStaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffDto> staff = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;


            ViewModel.Staffs = staff;


            return View(ViewModel);
        }

        /// <summary>
        /// Display Details of perticular staff
        /// select * from staff where id = 1
        /// </summary>
        /// <param name="id">staff id passing parameter</param>
        /// <returns>Details of selected staff details</returns>
        // GET: Staff/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// add staff page
        /// </summary>
        /// <returns>add new staff page</returns>
        // GET: Staff/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            GetApplicationCookie();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Staff staff)
        {
            GetApplicationCookie();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Staff staff)
        {
            GetApplicationCookie();
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
            GetApplicationCookie();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
