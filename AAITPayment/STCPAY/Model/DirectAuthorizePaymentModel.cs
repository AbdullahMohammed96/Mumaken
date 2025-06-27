using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAITPayment.STCPAY.Model
{
    public class DirectAuthorizePaymentModel
    {
        /// <summary>
        /// test: 966557877988
        /// should be registered with stc pay in ksa like vodafone cash in egypt
        /// </summary>
        public string PhoneNumber { get; set; } = "966557877988"; // for test
        public string Amount { get; set; }
        public string Note { get; set; }
    }
}
