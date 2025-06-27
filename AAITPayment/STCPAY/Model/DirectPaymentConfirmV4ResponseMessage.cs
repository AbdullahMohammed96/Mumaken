using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AAITPayment.STCPAY.Model
{
    public class DirectPaymentConfirmV4ResponseMessage
    {
        public string MerchantID { get; set; }
        public string BranchID { get; set; }
        public string TellerID { get; set; }
        public string DeviceID { get; set; }
        public string RefNum { get; set; }
        public string STCPayRefNum { get; set; }
        public string TokenId { get; set; }
        public double Amount { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PaymentDate { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
    }
}