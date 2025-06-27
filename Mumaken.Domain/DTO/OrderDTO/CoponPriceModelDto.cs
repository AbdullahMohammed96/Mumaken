using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class CoponPriceModelDto
    {
        public double priceForDailyRental { get; set; }
        public double varPrice { get; set; }
        public double vatPercetage { get; set; }
        public double subTotal { get; set; }
        public double netPrice { get; set; }
        public double discountPrice { get; set; }
        public int breakPeriod { get; set; }
        public double breakPrice { get; set; }
        public int dailyRentalCompanyPeriod { get; set; }
        public double dailyRentalCompanyPrice { get; set; }
    }
}
