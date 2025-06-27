using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.HyperPay.Model
{
    public static class PaymentMethods
    {
        public static int VISA { get; set; } = 1;
        public static int MASTER { get; set; } = 1;
        public static int MADA { get; set; } = 2;
        public static int APPLE { get; set; } = 3;
    }
}
