using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThePlanner.Models;
using ThePlanner.Models.MemberViewModel;

namespace ThePlanner.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context;
        ApplicationUserManager _userManager;

        public UserController()
        {

        }

        public UserController(ApplicationUserManager manager, ApplicationDbContext context)
        {
            _userManager = manager;
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

        [HttpGet]
        public ActionResult CompleteProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompleteProfile(AdditionalInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                if (user != null)
                {

                    if (Enum.IsDefined(typeof(Gender), model.Gender))
                    {
                        user.Gender = model.Gender;
                    }
                    if (string.IsNullOrEmpty(model.Phone) == false)
                    {
                        user.PhoneNumber = model.Phone;
                    }
                    if (model.Age > 0 && model.Age <= 170)
                    {
                        user.Age = (int)model.Age;
                    }
                    await DbContext.SaveChangesAsync();
                    return Redirect(Url.Action("Index","Home"));
                }
                else
                {
                    throw new NullReferenceException("Cannot find the user");
                }

            }
            else
            {
                return View(model);
            }
        }
    }
}