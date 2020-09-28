using Carreno_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.ViewModel
{
    public class ProjectViewModel
    {
        public int ProjectCount { get; set; }
        public List<Project> AllProjects { get; set; }
        public ICollection<ApplicationUser> AllPMs { get; set; }
        public ICollection<ApplicationUser> AllDevs { get; set; }
        public ICollection<ApplicationUser> AllSubs { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public Project Project { get; set; }
        public ICollection<Ticket> Tickets { get; set; }


        public ProjectViewModel()
        {
            AllProjects = new List<Project>();
            AllPMs = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Ticket>();
        }



    }
}