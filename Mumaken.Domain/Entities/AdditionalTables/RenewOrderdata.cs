using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.AdditionalTables
{
    public class RenewOrderdata
    {
        public int Id { get; set; }
        public DateTime DateRentalPeriod { get; set; }
        public DateTime DateEndRentalPeriod { get; set; }
        public DateTime CancelContractDate { get; set; }
        public int TypePay { get; set; }
        public int Duration { get; set; }
        public int  Status { get; set; }
        public int Case { get; set; }
        public bool IsPaid { get; set; }
        public string? CoponCode { get; set; }
        public double? coponPrice { get; set; }  // نسبة الخصم
        public double AppPercetage { get; set; }
        public double VatPercetage { get; set; }
        public double AppPrice { get; set; }  // Taken From Provider
        public double VatPrice { get; set; }  // Taken From Client
        public double NetPrice { get; set; }  // (سعر التاجير السياره لليوم الواحد +VatPrice) - coponPrice
        public double subTotal { get; set; }  //سعر التاجير السياره لليوم الواحد +VatPrice
        public double DiscountValueForDailyRental { get; set; }
        public int BreakPeriod { get; set; } //مده كسر التعاقد  
        public double BreakPeriodPrice { get; set; }
        public int RentalConfirmationDelayPeriod { get; set; }  //المده الفرق بين انهاء الطلب وتاكيد شركه التاجير
        public double RentalConfirmationDelayPrice { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
