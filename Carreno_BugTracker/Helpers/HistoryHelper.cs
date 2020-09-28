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
       public void ManageHistoryRecordCreation(Ticket oldTicket, Ticket newTicket)
        {

            if (oldTicket.Title != newTicket.Title)
            {

                var newHistoryRecord = new TicketHistory
                {
                    ChangedOn = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "DeveloperId",
                    OldValue = oldTicket.DeveloperId,
                    NewValue = newTicket.DeveloperId,
                    TicketId = newTicket.Id

                };

                db.TicketHistories.Add(newHistoryRecord);

            }

            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    ChangedOn = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "DeveloperId",
                    OldValue = oldTicket.DeveloperId,
                    NewValue = newTicket.DeveloperId,
                    TicketId = newTicket.Id
                };

                db.TicketHistories.Add(newHistoryRecord);
            
            }

            db.SaveChanges();
        }
    }
}