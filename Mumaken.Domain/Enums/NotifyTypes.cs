namespace Mumaken.Domain.Enums
{
    public enum NotifyTypes
    {
        BlockUser = -1,
        AcceptOrderByAdmin = 1,
        AcceptOrderByRentalCompany = 2,
        AcceptOrderByDeliveryCompany = 3,
        CanacelContact=4,
        PaymentOrder=5,
        RenewOrder=6,
        NotiyFromDashBord = 7,
        NewOrder=8,
        RefusedOrder=9,
        RefusedByDeliveryCompany=10,
        RefusedByRentalCompany=11,
        CancelContactByAdmin=12,
        EndRentalPeriod=13,
        ConfirmReciptCar=14,
        ConfirmReceiptAmount = 15,
        AddDeliveryCompanyLoginData=16,
        SendRequsetToFinishedContract = 17,
        SendRequsetToCancelContract = 18,
    }
}
