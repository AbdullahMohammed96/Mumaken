using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Notification
{
    public class HistoryNotificationViewModel
    {
        public string Text { get; set; }
        public DateTime TextDate { get; set; }
        public int NotifyCount { get; set; }
    }
}
