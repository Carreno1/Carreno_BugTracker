using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Carreno_BugTracker.Helpers;
using Carreno_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace Carreno_BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private RecordManager recordHelper = new RecordManager();

        //I want to include the Ticket Attachment functionality in my Ticket Dashboard
        public ActionResult Dashboard(int id)
        {

            return View(db.Tickets.Find(id));
        }








        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? projectId)
        {
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");

            //I need to first get my projects and then list them into selectlist

            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);



            if (projectId == null)
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");


            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            var newTicket = new Ticket();

            if (projectId != null)
            {
                newTicket.ProjectId = (int) projectId;
            }
            
            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                
               
                ticket.SubmitterId = User.Identity.GetUserId();
                ticket.Created = DateTime.Now;
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "New").Id;

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");

            //I need to first get my projects and then list them into selectlist

            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);



            if (ticket.ProjectId == 0)
                ViewBag.ProjectId  = new SelectList(myProjects,"Id","Name");


            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles ="Admin, Developer, Submitter, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);

            var currentUserId = User.Identity.GetUserId();

            //I need some additional, more granular security to determine whether 
            //"This is my ticket" depends on your role
            //If I am a Developer:
            var authorized = true; //or var authorized = new tickethelper(true)
            if ((User.IsInRole("Developer") && ticket.DeveloperId != currentUserId) ||
                (User.IsInRole("Submitter") && ticket.SubmitterId != currentUserId))
            {
                authorized = false;
            }

            
            

            //if (User.IsInRole("Project Manager"))
            //{
            //    var myTicketIds = db.Projects.Where(p => p.ProjectManagerId == currentUserId).SelectMany(p => p.Tickets).Select(t => t.Id);


            //}

            if (!authorized)
            {
                TempData["UnAuthorizedTicketAccess"] = $"You are not authorized to edit Ticket {id}";
                return RedirectToAction("Index", "Home");
            }


            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "Email", ticket.DeveloperId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,SubmitterId,DeveloperId,Title,Created,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);

                if (User.IsInRole("Developer") && userId == ticket.DeveloperId || User.IsInRole("Admin") || User.IsInRole("Project Manager") && ticket.Project.ProjectManagerId == userId || User.IsInRole("Submitter"))
                {

                    //I want to use AsNoTracking() to get a Memento Ticket object
                    var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);


                    ticket.Updated = DateTime.Now;

                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();

                    //Use the Histoy Helper to intelligently create the appropriate records
                    historyHelper.ManageHistoryRecordCreation(oldTicket, ticket);

                    //Use the Notification Helper to intelligently create the appropriate records
                    notificationHelper.ManageNotifications(oldTicket, ticket);
                    //recordHelper.ManageChangeRecords(oldTicket, ticket);

                    ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
                    ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                    ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
                    ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                    ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
                    ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                    return RedirectToAction("Index", "TicketHistories");


                }
                else
                {
                    return View("Error");
                }

        
            }
            return View(ticket);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
