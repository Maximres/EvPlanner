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
    /// <summary>
    /// Управление событиями
    /// </summary>
    public class OccasionController : Controller
    {
        private static readonly object lockObject = new object();
        ApplicationDbContext _context;
        ApplicationUserManager _userManager;

        public OccasionController()
        {

        }

        public OccasionController(ApplicationDbContext context, ApplicationUserManager manager)
        {
            _context = context;
            _userManager = manager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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

        [HttpPost]
        public async Task<ActionResult> Subscribe(int id)
        {
            var occassion = await DbContext
                .Occasions
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == id);
            var userName = HttpContext?.User?.Identity?.Name;

            if (occassion != null && userName != null)
            {
                var maxCount = occassion.MembersLimitCount;
                var currentUser = await UserManager.Users.FirstOrDefaultAsync(s => s.UserName == userName);

                lock (lockObject)
                {
                    var currentSubs = occassion.Members.Count;
                    if (maxCount > currentSubs)
                    {
                        if (occassion.Members.Contains(currentUser) == false)
                        {
                            occassion.Members.Add(currentUser);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Вы уже подписаны" });
                        }
                        
                    }
                    else
                    {
                        return Json(new { success = false, message = "Невозможно подписаться" });
                    }
                }
                try
                {
                    await DbContext.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Возникла ошибка при подписки"});
                }
            }
            else
            {
                return Json(new { success = true, message = "Невозможно подписать пользователя" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> Create(SimpleOccasionViewModel occassion, SimpleInputViewModel[] field)
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
                return Json(new { success = true });

            }
            catch (Exception)
            {
                return Json(new { success = false });

            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> Delete(int id)
        {
            var occassion = await DbContext.Occasions.FindAsync(id);
            if (occassion != null)
            {
                DbContext.Occasions.Remove(occassion);
                await DbContext.SaveChangesAsync();
                return Json(new { success = "true" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = "false" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Display(int id)
        {
            var occassion = await DbContext.Occasions
                .Include(x => x.InputFields)
                .Include(x => x.Members)
                .FirstOrDefaultAsync(o => o.Id == id);

                return PartialView(occassion ?? new Occasion());
        }

        public async Task<ActionResult> UpdateCalendar()
        {
            var evs = await DbContext
                .Occasions
                .Include("Members")
                .Include("InputFields")
                .ToListAsync();

            var dataSources = new List<DataSourceViewModel>();
            foreach (var item in evs)
            {
                dataSources.Add(new DataSourceViewModel
                {
                    id = item.Id,
                    name = item.Topic,
                    startDate = item.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    endDate = item.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    location = item.Location,
                    inputs = item.InputFields?.Select(s => new SimpleInputViewModel { Name = s.Name, Val = s.Value })?.ToList() ?? new List<SimpleInputViewModel>()
                    
                });
            }
            var converter = JsonConvert.SerializeObject(dataSources);
            return Json(dataSources, JsonRequestBehavior.AllowGet);
        }
    }
}