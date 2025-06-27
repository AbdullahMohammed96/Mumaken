using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.HyperPay.Model
{
    /// <summary>
    /// الموديل ده بتحصل عليه لما تعمل عمليه PayRequest
    /// </summary>
    public class PaymentResult
    {
        /// <summary>
        /// علشان توصل لقيمة checkoutId لازم الاول تعمل عمليه PayRequest()
        /// </summary>
        public string checkOutId { get; set; }
        //public string entityId { get; set; }
        //public string accessToken { get; set; }
        public bool isLive { get; set; }
        public string userId { get; set; }
        public int paymentMethod { get; set; }
    }
}
