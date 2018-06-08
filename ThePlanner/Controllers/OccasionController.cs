using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThePlanner.Models;
using ThePlanner.Models.OccasionViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity;
using Newtonsoft;
using Newtonsoft.Json;


namespace ThePlanner.Controllers
{
    public class OccasionController : Controller
    {
        ApplicationDbContext _context;

        public OccasionController()
        {

        }

        public OccasionController(ApplicationDbContext context)
        {
            _context = context;


        }

        public ApplicationDbContext DbContext
        {
            get
            {
                return _context ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _context = value;
            }
        }

        [Authorize(Roles = "user")]
        public async Task CreateOccasion(SimpleOccasionViewModel occassion, SimpleInputViewModel[] field)
        {

            var fields = new List<InputField>();
            if (field != null && field.Count() > 0)
            {
                foreach (var fld in field?.ToList())
                {
                    if (fld.Name != null && fld.Val != null)
                    {
                        fields.Add(new InputField { Name = fld.Name, Value = fld.Val });
                    }
                }

            }
            var time = occassion.Time;
            var date = occassion.Date;
            var day = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
            var ev = new Occasion
            {
                Date = day,
                Location = occassion.Location,
                Members = new List<ApplicationUser>(),
                InputFields = fields.Distinct().ToList(),
                MembersLimitCount = occassion.Count,
                MembersCount = 0,
                Topic = occassion.Topic,
                
            };
            DbContext.Occasions.Add(ev);
            try
            {
                await DbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO: response with json
            }
        }

        public async Task<ActionResult> DeleteOccassion(int id)
        {
            //TODO: удаление
            throw new NotImplementedException();
        }

        public async Task<ActionResult> UpdateCalendar()
        {
            var evs = await DbContext.Occasions.Include("Members").Include("InputFields").ToListAsync();
            var dataSources = new List<DataSourceViewModel>();
            foreach (var item in evs)
            {
                dataSources.Add(new DataSourceViewModel
                {
                    id = item.Id,
                    name = item.Topic,
                    startDate = item.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    endDate = item.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    location = item.Location
                    
                });
            }
            var converter = JsonConvert.SerializeObject(dataSources);
            return Json(dataSources, JsonRequestBehavior.AllowGet);
        }
    }
}