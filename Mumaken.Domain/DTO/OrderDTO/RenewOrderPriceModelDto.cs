using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class RenewOrderPriceModelDto
    {
        public double? coponPrice { get; set; }  // نسبة الخصم
        public double AppPercetage { get; set; }
        public double VatPercetage { get; set; }
        public double AppPrice { get; set; }  
        public double VatPrice { get; set; }  
        public double NetPrice { get; set; }  
        public double subTotal { get; set; }  
        public double DiscountValueForDailyRental { get; set; }
    }
}
