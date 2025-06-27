using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAITPayment.STCPAY.Model
{
    public class PaymentInquiryV4ResponseMessage
    {
        public List<TransactionList> TransactionList { get; set; }
    }
}