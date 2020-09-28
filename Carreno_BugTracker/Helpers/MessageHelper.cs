using Carreno_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carreno_BugTracker.Helpers
{
    public class MessageHelper
    {
      
        
        public static List<TicketNotification> GetUnreadNotifications()
        {

            var userId = HttpContext.Current.User.Identity.GetUserId();

            if (userId == null)
            {
                return new List<TicketNotification>();

            }
            var db = new ApplicationDbContext();
            return db.TicketNotifications.Where(t => t.RecipientId == userId && !t.IsRead).ToList();

        }

    }
}