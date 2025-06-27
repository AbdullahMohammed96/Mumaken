namespace Mumaken.Domain.Enums
{

    public enum OrderStutes
    {
        NewOrder = 1,
        Current=2,
        Finished=3,
        Refused = 4,  // refused by Provider
        Canceled =5,  // Canceled By Client

    }
}
