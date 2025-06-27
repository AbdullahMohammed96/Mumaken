using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAITPayment.STCPAY.Model
{
    public class DirectPaymentConfirmModel
    {
        public string OtpReference { get; set; }
        public string OtpValue { get; set; }
        public string StcPayPmtReference { get; set; }


    }
}
