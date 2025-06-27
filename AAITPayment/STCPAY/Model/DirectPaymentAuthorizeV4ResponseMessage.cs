using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAITPayment.STCPAY.Model
{
    public class DirectPaymentAuthorizeV4ResponseMessage
    {
        public string OtpReference { get; set; }
        public string STCPayPmtReference { get; set; }
        public int ExpiryDuration { get; set; }
    }
}