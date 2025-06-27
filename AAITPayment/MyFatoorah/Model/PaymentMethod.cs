using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah.Model
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethodAr { get; set; }
        public string PaymentMethodEn { get; set; }
        public bool IsDirectPayment { get; set; }
        public double ServiceCharge { get; set; }
        public double TotalAmount { get; set; }
        public string CurrencyIso { get; set; }
        public string ImageUrl { get; set; }
    }
}
