using Carreno_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.ViewModel
{
    public class DashboardViewModel
    {
        public int ProjectCount { get; set; }
        public int TicketCount { get; set; }
        public int UserCount { get; set; }
       
        public int HighPriorityTicketCount { get; set; }
        public int NewTicketCount { get; set; }
        public int TotalComments { get; set; }


        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public ICollection<ApplicationUser> Admins { get; set; }
        public ICollection<ApplicationUser> ProjectManagers { get; set; }
        public ICollection<ApplicationUser> Developers { get; set; }
        public ICollection<ApplicationUser> Submitters { get; set; }

        public ICollection<Project> AllProjects { get; set; }
        public List<Ticket> AllTickets { get; set; }

        public ProjectViewModel ProjectVM { get; set; }

        public DashboardViewModel()
        {
            AllProjects = new HashSet<Project>();
            AllTickets = new List<Ticket>();
          
            ApplicationUsers = new HashSet<ApplicationUser>();
            Admins = new List<ApplicationUser>();
            ProjectManagers = new List<ApplicationUser>();
            Developers = new List<ApplicationUser>();
            Submitters = new List<ApplicationUser>();
            //ProjectVM = new ProjectViewmodel();
        }

    }
}