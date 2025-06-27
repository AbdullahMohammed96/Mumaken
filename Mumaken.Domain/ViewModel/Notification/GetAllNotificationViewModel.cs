using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Notification
{
    public class GetAllNotificationViewModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public string UserName { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DateSend { get; set; }
    }
}
