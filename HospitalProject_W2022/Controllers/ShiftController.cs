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
    public class ShiftController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ShiftController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44377/api/ShiftData/");
        }

        /// <summary>
        /// Display list of all shifts
        /// select * from shifts
        /// </summary>
        /// <returns>List of all shifts</returns>
        // GET: Shift/List
        public ActionResult List()
        {
            string url = "ListShifts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ShiftDto> shiftDtos = response.Content.ReadAsAsync<IEnumerable<ShiftDto>>().Result;
            return View(shiftDtos);
        }

        /// <summary>
        /// Display Details of perticular shift
        /// select * from shift where id = 1
        /// </summary>
        /// <param name="id">shift id passing parameter</param>
        /// <returns>Details of selected shift</returns>
        // GET: Shift/Details/5
        public ActionResult Details(int id)
        {
            string url = "FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ShiftDto selectedShiftDto = response.Content.ReadAsAsync<ShiftDto>().Result;
            return View(selectedShiftDto);
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>Error page</returns>
        // GET: Shift/Error
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// add shift page
        /// </summary>
        /// <returns>add new shift page</returns>
        // GET: Shift/New
        public ActionResult New()
        {
            //need staff information for staff dropdown
            string url = "ListStaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> staffOptions = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            return View(staffOptions);
        }

        /// <summary>
        /// create new shift
        /// insert into shift (date, type) values(March 21,2022, Morning)
        /// </summary>
        /// <param name="shift"></param>
        /// <returns>added record into shift table</returns>
        // POST: Shift/Create
        [HttpPost]
        public ActionResult Create(Shift shift)
        {
            string url = "AddShift";

            string jsonpayload = jss.Serialize(shift);

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
        /// Display Edit page for perticular shift
        /// </summary>
        /// <param name="id">shift id</param>
        /// <returns>edit page</returns>
        // GET: Shift/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateShift viewModel = new UpdateShift();
            
            //selected shift information
            string url = "FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto selectedShiftDto = response.Content.ReadAsAsync<ShiftDto>().Result;
            viewModel.SelectedShift = selectedShiftDto;

            //list of staff dropdown 
            url = "ListStaffs";
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> staffOptions = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            viewModel.StaffOptions = staffOptions;

            return View(viewModel);
        }

        /// <summary>
        /// update shift details into database
        /// update shift set date=@date, type=@type where id=1
        /// </summary>
        /// <param name="id">shift id</param>
        /// <param name="shift">shift details such as date, and type</param>
        /// <returns>update information into shift table for perticular shift</returns>
        // POST: Shift/Update/5
        [HttpPost]
        public ActionResult Update(int id, Shift shift)
        {
            string url = "UpdateShift/" + id;

            string jsonpayload = jss.Serialize(shift);

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
        /// display delete confirmation page for perticular shift
        /// </summary>
        /// <param name="id">shift id</param>
        /// <returns>delete confirm page</returns>
        // GET: Shift/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ShiftDto selectedShiftDto = response.Content.ReadAsAsync<ShiftDto>().Result;
            return View(selectedShiftDto);
        }

        /// <summary>
        /// delete record from database
        /// delete from shift where id = 1
        /// </summary>
        /// <param name="id">shift id</param>
        /// <returns>delete perticular shift record from the shift table </returns>
        // POST: Shift/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteShift/" + id;
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
