namespace Mumaken.Domain.Model
{
    public class DashBoardHomeModel
    {
        public int UserCount { get; set; }
        public int DriverCount { get; set; }
        public int IndependentDriversCount { get; set; }

        public int CompanyDriversCount { get; set; }

        public int DeliveryCompanyCount { get; set; }
        public int RentalCompanyCount { get; set; }
        public int CityCount { get; set; }
        public int totalOrder { get; set; }
        public int totalNewOrder { get; set; }
        public int totalCurrentOrder { get; set; }
        public int totalCanceledOrder { get; set; }
    }
}
