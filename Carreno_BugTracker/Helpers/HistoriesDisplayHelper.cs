using Carreno_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.Helpers
{
    public class HistoriesDisplayHelper
    {
        public static string DisplayData(TicketHistory ticketHistory)
        {
            var db = new ApplicationDbContext();
            var data = "";

            switch (ticketHistory.Property)
            {
                case "DeveloperId":
                    data = db.Users.FirstOrDefault(u => u.Id == ticketHistory.NewValue).FullName;
                    break;
                default:
                    data = ticketHistory.NewValue;
                    break;

            }
            return data;
        }


    }
}