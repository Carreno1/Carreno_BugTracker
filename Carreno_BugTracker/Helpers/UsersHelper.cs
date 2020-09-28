using Carreno_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Carreno_BugTracker.Helpers
{
    public class AssignmentHelper
    {
        private RolesHelper roleHelper = new RolesHelper();
        private ProjectsHelper projHelper = new ProjectsHelper();

        public ICollection<ApplicationUser> UsersOnProjectInRole(int projectId, string roleName)
        {
            var users = new List<ApplicationUser>();

            var projUsers = projHelper.UsersOnProject(projectId);
            foreach (var user in projUsers)
            {
                if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == roleName)
                    users.Add(user);
            }
            return users;
        }

    }

    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string UserId { get; set; }

        public UserHelper()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId();
        }

        public string GetUserDisplayName()
        {
            return db.Users.Find(UserId).FirstName;
        }

        public string GetUserAvatar()
        {
            return db.Users.Find(UserId).AvatarPath;
        }
    }
}