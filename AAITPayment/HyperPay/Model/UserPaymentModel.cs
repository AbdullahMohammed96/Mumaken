using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.HyperPay.Model
{
    public class UserPaymentModel
    {

        public string Email { get; set; }
        /// <summary>
        /// اسم المستخدم
        /// </summary>
        public string GivenName { get; set; }
        /// <summary>
        /// اسم المستخدم
        /// </summary>
        public string SurName { get; set; }
        /// <summary>
        /// الدولة ب alpha-2 code
        /// SA : KSA
        /// 
        /// SY : SYRIA
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// اسم المدينة
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// الرمز البريدي
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// الشارع
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// الحي 
        /// </summary>
        public string State { get; set; }
        public string Amount { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// طريقة الدفع
        /// 
        /// 1 - VISA - MASTERCARD
        /// 
        /// 2 - MADA
        /// 
        /// 3 - APPLEPAY
        /// </summary>
        public int PaymentMethod { get; set; }

    }
}
