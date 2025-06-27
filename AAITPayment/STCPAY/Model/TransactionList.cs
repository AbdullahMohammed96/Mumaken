using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAITPayment.STCPAY.Model
{
    public class TransactionList
    {
        public string MerchantID { get; set; }
        public string BranchID { get; set; }
        public string TellerID { get; set; }
        public string DeviceID { get; set; }
        public string RefNum { get; set; }
        public string STCPayRefNum { get; set; }
        public double Amount { get; set; }
        public double AmountReversed { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
    }
}