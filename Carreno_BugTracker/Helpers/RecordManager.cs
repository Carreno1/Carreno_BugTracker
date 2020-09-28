using Carreno_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carreno_BugTracker.Helpers
{
    public class RecordManager
    {
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        public void ManageChangeRecords(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);
            notificationHelper.ManageNotifications(oldTicket, newTicket);

        }

    }
}