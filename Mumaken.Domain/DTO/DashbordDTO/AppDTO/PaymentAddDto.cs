using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.DashbordDTO.AppDTO
{
    public class PaymentAddDto
    {
        public string ViMaEntityId { get; set; }
        public string UserId { get; set; }
        public string MadaEntityId { get; set; }
        public string LiveAccessToken { get; set; }
        public string TestAccessToken { get; set; }
        public string PaymentType { get; set; } = "DB";
        public string Currency { get; set; } // SAR
        public bool IsLive { get; set; } // هل المشروع فمرحلة التست او اللايف
        public bool IsMada { get; set; } // هل داخل ادفع بمدى او فيزا
        public string LiveCheckoutUrl { get; set; } = "https://oppwa.com/v1/checkouts";
        public string TestCheckoutUrl { get; set; } = "https://test.oppwa.com/v1/checkouts";
    }
}
