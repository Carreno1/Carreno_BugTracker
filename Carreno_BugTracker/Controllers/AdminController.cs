using Carreno_BugTracker.Helpers;
using Carreno_BugTracker.Models;
using Carreno_BugTracker.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carreno_BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {

            var viewData = new List<CustomUserData>();
            var users = db.Users.ToList();

            foreach (var user in users)
            {
                //    //var newUserData = new UserRoleData();

                //    //newUserData.FirstName = user.FirstName;
                //    //newUserData.LastName = user.LastName;
                //    //newUserData.FirstName = user.FirstName;
                //    //newUserData.Email = user.Email;
                //    //newUserData.RoleName - roleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "UnAssigned";

                //}

                var newUserData = new CustomUserData();
                newUserData.FirstName = user.FirstName;
                newUserData.LastName = user.LastName;
                newUserData.Email = user.Email;
                newUserData.RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "UnAssigned";

                viewData.Add(newUserData);
            }
            //Setup some data that can be used inside the view

            //Left hand side control: This data will be used to power ListBox in the View
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            //Right hand side control: This data will be used to power a Dropdown List in the View
            ViewBag.RoleName = new MultiSelectList(db.Roles, "Name", "Name");


            return View(viewData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            //Use our RoleHelper to actually assign the role to the person or persons selected
            //This action is intended to operate on the selected Users.
            //If no Users were selected then there is nothing to do
            if (userIds != null)
            {
                //This loop removes all the selected Users from the Role they occupy
                //If they don't currently occupy a Role then I juess we are wasting time
                //trying to remove them from no role
                foreach (var userId in userIds)
                {

                    //First I need to determine what Role if any the user is in
                    var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(userRole))
                    {
                        //Second I have to remove each of the selected users from their current Role

                        roleHelper.RemoveUserFromRole(userId, userRole);

                    }

                    //Then I will add each selected user to the selected Role


                    if (!string.IsNullOrEmpty(roleName))
                    {

                        roleHelper.AddUserToRole(userId, roleName);

                    }

                }

            }

            return RedirectToAction("ManageRoles");
        }




    }
}