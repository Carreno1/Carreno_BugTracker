using Carreno_BugTracker.Helpers;
using Microsoft.Owin.Security.DataHandler.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.Models
{
    public class Project
    {
        private RolesHelper roleHelper = new RolesHelper();

        public Project()
        {
            Users = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectManagerId { get; set; }
         
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool isArchived { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public ICollection<ApplicationUser> GetProjectManager()
        {
            var pm = Users.Where(u => roleHelper.ListUserRoles(u.Id).FirstOrDefault() == "Project Manager").ToList();
            return pm;
        }
        public ICollection<ApplicationUser> GetDevelopers()
        {
            var devs = Users.Where(u => roleHelper.ListUserRoles(u.Id).FirstOrDefault() == "Developer").ToList();
            return devs;
        }
        public ICollection<ApplicationUser> GetSubmitters()
        {
            var subs = Users.Where(u => roleHelper.ListUserRoles(u.Id).FirstOrDefault() == "Submitter").ToList();
            return subs;
        }


    }
}