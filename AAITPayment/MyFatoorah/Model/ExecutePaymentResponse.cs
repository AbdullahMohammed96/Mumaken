using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah.Model
{
    public class ExecutePaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
        public DataResponse Data { get; set; }
    }
}
