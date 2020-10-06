using Carreno_BugTracker.Helpers;
using Carreno_BugTracker.Models;
using Carreno_BugTracker.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Carreno_BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private RolesHelper roleHelper = new RolesHelper();



        public ActionResult Index(DashboardViewModel dashview)
        {

            
            
            var allUsers = db.Users.ToList();

            var user = db.Users.Find(User.Identity.GetUserId());

            if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == "Admin")
            {
                dashview.ApplicationUsers = allUsers;
                dashview.UserCount = allUsers.Count();
                dashview.AllProjects = db.Projects.ToList();
                dashview.AllTickets = db.Tickets.ToList();
               
            }
            else if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == "Project Manager")
            {
                dashview.ApplicationUsers = allUsers;
                dashview.AllProjects = projHelper.ListUserProjects(user.Id);
                dashview.AllTickets = projHelper.ListUserProjects(user.Id).SelectMany(p => p.Tickets).ToList();
            }
            else if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == "Developer")
            {
                dashview.AllProjects = projHelper.ListUserProjects(user.Id);
                dashview.AllTickets = ticketHelper.ListMyTickets();
            }
            else if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == "Submitter")
            {
                dashview.AllProjects = projHelper.ListUserProjects(user.Id);
                dashview.AllTickets = ticketHelper.ListMyTickets();
            }


            //var dashboardVm = new DashboardViewModel()
            //{



            //    UserCount = allUsers.Count(),
            //    ApplicationUsers = db.Users.ToList(),


            //    HighPriorityTicketCount = allTickets.Where(t => t.TicketPriority.Name == "High").Count(),
            //    NewTicketCount = allTickets.Where(t => t.TicketStatus.Name == "New").Count(),
            //    TotalComments = db.TicketComments.Count(),
            //    AllProjects = Projects.ToList(),
            //    AllTickets = db.Tickets.ToList()





            //};

            //dashboardVm.ProjectVM.ProjectCount = 5;
            //dashboardVm.ProjectVM.AllProjects = db.Projects.ToList();
            //dashboardVm.ProjectVM.AllPMs = roleHelper.UsersInRoles("Project Manager").ToList();



            return View(dashview);
        }
        public ActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactAsync(EmailModel model)
        {
            try
            {


                //Spin up an instance of the EmailService Class and then call its SendEmailAsync method
                var emailAddress = WebConfigurationManager.AppSettings["EmailTo"];
                var emailFrom = $"{model.From}<{emailAddress}>";
                var FinalBody = $"{model.Name} has send you the following message {model.Body} {WebConfigurationManager.AppSettings["LegalText"]}";

                var email = new MailMessage(emailFrom, emailAddress)
                {
                    Subject = model.Subject,
                    Body = FinalBody,
                    IsBodyHtml = true
                };

                //build the Mail message that the SendEmailAsync method takes 
                var emailSvc = new EmailService();
                await emailSvc.SendAsync(email);


                return View(new EmailModel());


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }


            return View(new EmailModel());
        }


        public ActionResult UserProfile(string userId)
        {
           
            UserProfileVM model = new UserProfileVM();
           
            //var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (userId == null)
            {
                user = db.Users.Find(User.Identity.GetUserId());
            }
            model.User = user;
            model.UserRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            model.Tickets = ticketHelper.ListMyTickets();
            model.ProjectIn = projHelper.ListUserProjects(userId);
           

            //model.ProjectIn = projHelper.ListUserProjects(userId);
            //model.ProjectOut = projHelper.ListOtherProjects(userId);
            //model.TicketsIn = ticketHelper.ListMyTickets();
            //model.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();


            return View(model);

        }

        [Authorize]
        public ActionResult Users(DashboardViewModel dashview)
        {
            var allUsers = db.Users.ToList();

            dashview.ApplicationUsers = allUsers;

            dashview.Admins = allUsers.Where(u => roleHelper.IsUserInRole(u.Id, "Admin")).ToList();
            dashview.ProjectManagers = allUsers.Where(u => roleHelper.IsUserInRole(u.Id, "Project Manager")).ToList();
            dashview.Developers = allUsers.Where(u => roleHelper.IsUserInRole(u.Id, "Developer")).ToList();
            dashview.Submitters = allUsers.Where(u => roleHelper.IsUserInRole(u.Id, "Submitter")).ToList();


            return View(dashview);
        }


    }
}