using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAITPayment.STCPAY.Model
{
    public class MainDirectPaymentAuthorizeV4ResponseMessage
    {
        public DirectPaymentAuthorizeV4ResponseMessage DirectPaymentAuthorizeV4ResponseMessage { get; set; }
        public int Code { get; set; }
        public string Text { get; set; } = "";
        public int Type { get; set; }

    }
}