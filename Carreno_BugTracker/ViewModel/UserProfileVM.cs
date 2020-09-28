using Carreno_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.ViewModel
{
    public class UserProfileVM
    {



        public ApplicationUser User { get; set; }
        public ICollection<Project> ProjectIn { get; set; } //Projects I am assigned to
        public ICollection<Project> ProjectOut { get; set; } //Projects I am not assigned to
        public ICollection<Ticket> Tickets { get; set; } //Tickets I am involved in

        public string UserRole { get; set; }

        public UserProfileVM()
        {
            User = new ApplicationUser();
            ProjectIn = new HashSet<Project>();
            Tickets = new HashSet<Ticket>();

        }

    }
}
