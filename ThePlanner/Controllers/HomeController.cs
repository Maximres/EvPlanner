using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThePlanner.Infrastructure;
using ThePlanner.Models;

namespace ThePlanner.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public HomeController()
        {

        }

        public HomeController(ApplicationSignInManager signIn, ApplicationUserManager user, ApplicationRoleManager role, ApplicationDbContext dataBase)
        {
            _context = dataBase;
            _signInManager = signIn;
            _userManager = user;
            _roleManager = role;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationDbContext DbContext
        {
            get
            {
                return _context ?? new ApplicationDbContext();
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        [AdditionalInfo]
        public ActionResult Index()
        {
#if DEBUG
            var roles = DbContext.Roles.ToList();
            foreach(var item in roles)
            {
                System.Diagnostics.Debug.WriteLine(item.Name);
            }
#endif
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AddInfo]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}