using Carreno_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
              
            }
        }
    }
}