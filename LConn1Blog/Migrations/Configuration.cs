namespace LConn1Blog.Migrations
{
    using LConn1Blog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LConn1Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LConn1Blog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            #region Roles
            if (!context.Roles.Any(roles => roles.Name == "Admin"))
            {
                IdentityRole adminRole = new IdentityRole()
                {
                    Name = "Admin"
                };
                roleManager.Create(adminRole);
            }
            if (!context.Roles.Any(roles => roles.Name == "Moderator"))
            {
                IdentityRole moderRole = new IdentityRole()
                {
                    Name = "Moderator"
                };

                roleManager.Create(moderRole);
            }
            #endregion

            #region Users
            if (!context.Users.Any(roles => roles.UserName == "admin@myblog.com"))
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = "admin@myblog.com",
                    Email = "admin@myblog.com"
                };

                userManager.Create(adminUser, "P@ssw0rd");
            }
            if (!context.Users.Any(roles => roles.UserName == "moderator@myblog.com"))
            {
                ApplicationUser moderUser = new ApplicationUser()
                {
                    UserName = "moderator@myblog.com",
                    Email = "moderator@myblog.com"
                };

                userManager.Create(moderUser, "P@ssw0rd");
            }
            #endregion

            #region AssignToRoles
            var adminU = userManager.FindByEmail("admin@myblog.com");
            if (adminU != null)
            {
                userManager.AddToRole(adminU.Id, "Admin");
            }
            var moderU = userManager.FindByEmail("moderator@myblog.com");
            if (moderU != null)
            {
                userManager.AddToRole(moderU.Id, "Moderator");
            }
            #endregion
        }
    }
}