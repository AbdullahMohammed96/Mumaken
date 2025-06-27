using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAITPayment.STCPAY.Model
{
    public class DirectPaymentConfirmV4RequestMessage
    {
        public string OtpReference { get; set; }
        public string OtpValue { get; set; }
        public string STCPayPmtReference { get; set; }
        public string TokenReference { get; set; }
        public string TokenizeYn { get; set; }
    }
}