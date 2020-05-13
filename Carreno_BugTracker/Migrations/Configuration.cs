namespace Carreno_BugTracker.Migrations
{
    using Carreno_BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Carreno_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Carreno_BugTracker.Models.ApplicationDbContext context)
        {
            #region Create a Role Manager 



            //  This method will be called after migrating to the latest version.
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            #endregion 

            #region Add user creation here

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.Email == "michaelcarrenocc@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "michaelcarrenocc@gmail.com",
                    Email = "michaelcarrenocc@gmail.com",
                    FirstName = "Michael",
                    LastName = "Carreno",

                };

                userManager.Create(user, "Abc&123!");
                userManager.AddToRole(user.Id, "Admin");





            }

            #endregion

            #region load up ticket types

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType { Name = "Defect" },
                new TicketType { Name = "New Functionality" },
                new TicketType { Name = "Training" },
                new TicketType { Name = "Videos" }
                );
            #endregion

            #region load up ticket priorities

            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                new TicketPriority { Name = "Immediate" },
                new TicketPriority { Name = "High" },
                new TicketPriority { Name = "Medium" },
                new TicketPriority { Name = "Low" },
                new TicketPriority { Name = "None" }

                );
            #endregion

            #region load up ticket status

            context.TicketStatus.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Name = "New" },
                new TicketStatus { Name = "Assigned" },
                new TicketStatus { Name = "Unassigned" },
                new TicketStatus { Name = "Completed" },
                new TicketStatus { Name = "Archived" }
                );
            #endregion




        }
    }
}
