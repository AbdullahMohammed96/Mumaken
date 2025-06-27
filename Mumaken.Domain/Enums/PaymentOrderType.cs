using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Enums
{
    public enum PaymentOrderType
    {
       PaymentOrderToEndRentalPeriod=1,
       PaymentAndRenewPeriod=2,
       CancelContract=3
    }
}
