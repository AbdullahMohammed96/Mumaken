using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.NotificationDTO
{
    public class RateProviderNotifyDto
    {
        public string lang { get; set; } = "ar";
        public string providerId { get; set; }
        public string userName { get; set; }
    }
}
