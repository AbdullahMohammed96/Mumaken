using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class GetRentalCompanyReportViewModel
    {
        public int Id { get; set; }
        public string  CarNumber { get; set; }
        public int RentalPeriod { get; set; }
        public string DateStartPeriod { get; set; }
        public double DailyPriceForRental { get; set; }
        public double NetPrice { get; set; }

    }
}
