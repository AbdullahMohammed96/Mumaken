using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAITPayment.STCPAY.Model
{
    public class DirectPaymentAuthorizeV4RequestMessage
    {
        public string BranchID { get; set; }
        public string TellerId { get; set; }
        public string DeviceId { get; set; }
        public string RefNum { get; set; }
        public string BillNumber { get; set; }
        public string MobileNo { get; set; }
        public string Amount { get; set; }
        public string MerchantNote { get; set; }
    }
}