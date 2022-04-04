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
            client.BaseAddress = new Uri("https://localhost:44377/api/");
        }

        /// <summary>
        /// Display list of all shifts
        /// select * from shifts
        /// </summary>
        /// <returns>List of all shifts</returns>
        // GET: Shift/List
        public ActionResult List()
        {
            string url = "ShiftData/ListShifts";
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
            DetailsShift viewModel = new DetailsShift();

            //communicate wth data controller class
            string url = "ShiftData/FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto SelectedShift = response.Content.ReadAsAsync<ShiftDto>().Result;

            viewModel.SelectedShift = SelectedShift;

            url = "StaffData/ListStaffsForShift/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> KeptStaffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            viewModel.KeptStaff = KeptStaffs;

            url = "StaffData/ListStaffsNotInPerticularShift/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> StaffOptions = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            viewModel.StaffOptions = StaffOptions;

            return View(viewModel);
        }

        /// <summary>
        /// add new staff in the perticular shift
        /// </summary>
        /// <param name="id">passing parameter shift id</param>
        /// <param name="StaffID">passing parameter staff id</param>
        /// <returns>add staff in the perticular shift</returns>
        //POST: Shift/Associate/{id}
        [HttpPost]
        public ActionResult Associate(int id, int SID)
        {
            //communicate with data controller class
            string url = "ShiftData/AssociateShiftWithStaff/" + id + "/" + SID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        /// <summary>
        /// remove staff from the perticular shift
        /// </summary>
        /// <param name="id">passing parameter shift id</param>
        /// <param name="StaffID">passing parameter staff id</param>
        /// <returns>remove staff from perticular shift</returns>
        //GET: Shift/UnAssociate/{id}?SID={SID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int SID)
        {
            //communicate with data controller class
            string url = "ShiftData/UnAssociateShiftWithStaff/" + id + "/" + SID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
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
            return View();
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
            string url = "ShiftData/AddShift";

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
            string url = "ShiftData/FindShift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ShiftDto selectedShiftDto = response.Content.ReadAsAsync<ShiftDto>().Result;
            viewModel.SelectedShift = selectedShiftDto;

            //list of staff dropdown 
            url = "StaffData/ListStaffsForShift/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> KeptStaffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            viewModel.KeptStaff = KeptStaffs;

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
            string url = "ShiftData/UpdateShift/" + id;

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
            string url = "ShiftData/FindShift/" + id;
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
            string url = "ShiftData/DeleteShift/" + id;
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
