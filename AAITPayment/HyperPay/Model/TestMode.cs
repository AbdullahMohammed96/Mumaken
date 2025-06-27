using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.HyperPay.Model
{
    public static class TestMode
    {
        public static string INTERNAL { get; set; } = "INTERNAL";
        public static string EXTERNAL { get; set; } = "EXTERNAL";
    }
}
