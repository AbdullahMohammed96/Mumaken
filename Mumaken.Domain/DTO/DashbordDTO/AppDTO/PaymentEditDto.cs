using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.DashbordDTO.AppDTO
{
    public class PaymentEditDto
    {
        public int Id { get; set; }
        public string ViMaEntityId { get; set; }
        public string MadaEntityId { get; set; }
        public string LiveAccessToken { get; set; }
        public string TestAccessToken { get; set; }
        public string PaymentType { get; set; }
        public string Currency { get; set; }
        public bool IsLive { get; set; }
        public bool IsMada { get; set; }
        public string LiveCheckoutUrl { get; set; }
        public string TestCheckoutUrl { get; set; }
        public string UserId { get; set; }
    }
}
