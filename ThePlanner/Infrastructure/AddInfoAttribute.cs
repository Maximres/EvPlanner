using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThePlanner.Models;

namespace ThePlanner.Infrastructure
{
    //TODO: roll this attr
    public class AddInfoAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var userName = filterContext.HttpContext?.User?.Identity?.Name;
            if (userName != null)
            {
                var context = filterContext.HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var user = context.Users.FirstOrDefault(x => x.UserName == userName);
                if (user != null && user.PhoneNumber is null)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "User", action = "CompleteProfile" }));
                    return;
                }
            }
        }
    }
}