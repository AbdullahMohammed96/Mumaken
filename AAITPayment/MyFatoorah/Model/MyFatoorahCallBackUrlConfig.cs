using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah.Model
{
    public class MyFatoorahCallBackUrlConfig
    {
        public string LiveHostServer { get; set; }
        public string ControllerName { get; set; }
        public string SuccessActionName { get; set; }
        public string FailActionName { get; set; }
        public string Amount { get; set; }
        public string QueryParamsRouteValues { get; set; }

    }


}
