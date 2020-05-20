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


     
        public ActionResult Index()
        {
            return View();
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


        public ActionResult UserProfile()
        {
            var model = new UserProfileVM();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            model.AvatarPath = user.AvatarPath;
            model.Email = user.Email;
            model.FullName = user.FullName;
            model.Id = userId;
            //model.ProjectIn = projHelper.ListUserProjects(userId);
            //model.ProjectOut = projHelper.ListOtherProjects(userId);
            model.TicketsIn = ticketHelper.ListMyTickets();
            model.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();


            return View(model);

        }



    }
}