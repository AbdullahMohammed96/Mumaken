using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah.Model
{
    public class ExecutePaymentModel
    {
        public string PaymentMethodId { get; set; }
        //public string CustomerName { get; set; }
        //public string DisplayCurrencyIso { get; set; }
        //public string MobileCountryCode { get; set; }
        //public string CustomerMobile { get; set; }
        //public string CustomerEmail { get; set; }
        public int InvoiceValue { get; set; }
        public string CallBackUrl { get; set; }
        public string ErrorUrl { get; set; } = "https://specialgiftt.com/Payments/Fail";
        public string Language { get; set; } = "AR";
        //public string CustomerReference { get; set; }
        //public int CustomerCivilId { get; set; }
        //public string UserDefinedField { get; set; }
        //public string ExpireDate { get; set; }
        //public CustomerAddress CustomerAddress { get; set; }
        //public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
