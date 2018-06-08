namespace ThePlanner.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ThePlanner.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ThePlanner.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ThePlanner.Models.ApplicationDbContext";
        }

        protected override void Seed(ThePlanner.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            ApplicationUserManager userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            ApplicationRoleManager roleMgr = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            ApplicationDbContext db = new ApplicationDbContext();


            string adminRoleName = "admin";
            string userRoleName = "user";
            Occasion occasion1 = new Occasion
            {
                Date = DateTime.Now.Add(new TimeSpan(5, 5, 5, 5)),
                Location = "Downtown area, Minsk, Belarus",
                MembersCount = 0,
                MembersLimitCount = 5,
                Topic = "Meeting",
                Members = new List<ApplicationUser>() { userMgr.Users.FirstOrDefault() }
            };

            if (null == context.Occasions.FirstOrDefault(s => s.Location == "Downtown area, Minsk, Belarus"))
            {
                context.Occasions.Add(occasion1);
            }
            //admin
            string aEmail = "admin@example.com";

            if (!roleMgr.RoleExists(adminRoleName))
            {
                roleMgr.Create(new ApplicationRole(adminRoleName));
            }

            if (!roleMgr.RoleExists(userRoleName))
            {
                roleMgr.Create(new ApplicationRole(userRoleName));
            }

            ApplicationUser adm = userMgr.FindByEmail(aEmail);
            if (adm == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = aEmail, UserName = aEmail, EmailConfirmed = true };
                var result = userMgr.Create(admin, "ABCdef123");
                if (result.Succeeded)
                {
                    adm = userMgr.FindByEmail(aEmail);
                }
            }

            if (!userMgr.IsInRole(adm.Id, adminRoleName))
            {
                userMgr.AddToRole(adm.Id, adminRoleName);
            }

            if (!userMgr.IsInRole(adm.Id, userRoleName))
            {
                userMgr.AddToRole(adm.Id, userRoleName);
            }

            //save
            context.SaveChanges();
        }
    }
}
