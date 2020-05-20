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
using Carreno_BugTracker.ViewModel;

namespace Carreno_BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private RolesHelper roleManager = new RolesHelper();
        private AssignmentHelper assignHelper = new AssignmentHelper();

        [Authorize (Roles = "Admin, Project Manager")]
        public ActionResult AssignUsers(int projectId)
        {

            ViewBag.ProjectId = projectId;
            if (User.IsInRole("Admin"))
            {
                ViewBag.ProjectManagerId = new SelectList(roleManager.UsersInRoles("Project Manager"), "Id", "FullName");
            }
            else
            {
                ViewBag.SubmitterIds = new MultiSelectList(roleManager.UsersInRoles("Submitter"), "Id", "FullName");
                ViewBag.DeveloperIds = new MultiSelectList(roleManager.UsersInRoles("Developer"), "Id", "FullName");

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsers(int projectId, string projectManagerId, List<string> submitterIds, List<string> developerIds)
        {
            if (User.IsInRole("Admin"))
            {
                //Dealing with Project Managers
                //Remove the current PM and then add the selected PM
                var project = db.Projects.Find(projectId);
                project.ProjectManagerId = projectManagerId;
                db.SaveChanges();
            }
            else
            {
                #region Handle Submitters Project association
               
                //Remove all Submitters on the Project
                foreach (var user in assignHelper.UsersOnProjectInRole(projectId, "Submitter"))
                {
                    projHelper.RemoveUserFromProject(user.Id, projectId);
                }

                //Add back all the selected Submitters
                if (submitterIds != null)
                {
                    foreach (var submitterid in submitterIds)
                    {
                        projHelper.AddUserToProject(submitterid, projectId);
                    }
                }
                #endregion

                #region Handle Developer Project association
                //Add back all the selected Developers

                foreach (var user in assignHelper.UsersOnProjectInRole(projectId, "Developer"))
                {
                    projHelper.RemoveUserFromProject(user.Id, projectId);
                }

                //Add back all the selected Submitters
                if (developerIds != null)
                {
                    foreach (var developerId in developerIds)
                    {
                        projHelper.AddUserToProject(developerId, projectId);
                    }
                }

                #endregion
            }

            return RedirectToAction("Index", "Home");
        }



        [Authorize(Roles = "Project Manager, Admin")]
        public ActionResult ManageProjectAssignments()
        {

            var emptyCustomUserDataList = new List<CustomUserData>();

            var users = db.Users.ToList();
            //I have decided that it should work in this way
            //Load up Multi Select List of Users
            ViewBag.UserIds = new MultiSelectList(users, "Id", "FullName");
            //Load up a Multi Select List of Projects
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            //Load up the View Model
            foreach (var user in users)
            {
                emptyCustomUserDataList.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()

                });

            }


            return View(emptyCustomUserDataList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectAssignments(List<string> userIds, List<int> projectIds, bool addUser)
        {
            //If and only if I have chosen at least one person will I do the following operations..
            if (userIds != null || projectIds != null)
            {

                //I can simply Add each of the selected Users to each of the Selected Projects
                if (addUser)
                {
                    foreach (var userId in userIds)
                    {
                        foreach (var projectId in projectIds)
                        {
                            projHelper.RemoveUserFromProject(userId, projectId);

                        }
                    }
                    foreach (var userId in userIds)
                    {
                        foreach (var projectId in projectIds)
                        {
                            projHelper.AddUserToProject(userId, projectId);

                        }
                    }
                }
                else
                {
                    foreach (var userId in userIds)
                    {
                        foreach (var projectId in projectIds)
                        {
                            projHelper.RemoveUserFromProject(userId, projectId);

                        }
                    }


                }
            }


            return RedirectToAction("ManageProjectAssignments");



        }







        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectManagerId,Updated,isArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                project.Created = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectManagerId,Created,Updated,isArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
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
