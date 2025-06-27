using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Enums
{
    public enum OrderCase
    {
        WaitAcceptAdministration=1,
        WaitToAcceptRentalCompany=2,
        WaitToAcceptDeliverCompany=3,
        FinishedRentalPeriodAndNotRenew=4,
        RenewRentalPeriod=5,
        Accepted=6,
        Finished=7,
        Canceled=8,  // By Client
        Refused=9,  // By Provider
        CancelContractAndPaidRentalPeriod =10,// by client
        CancelContractByAdminAndNotPaidRentalPeriod =11,
        CancelContractByAdminAndPaidRentalPeriod =12,
        SendRequsetToFinishedContract=13,
        SendRequsetToCancelContract=14,
        ConfirmReciptCar=15,  // تاكد استلام السياره
        PaymentOrderAndWaitConfirmRentanlCompany=16,
    }
}
