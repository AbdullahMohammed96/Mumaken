using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.Chat;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mumaken.Domain.Entities.AdditionalTables
{
    public class Order
    {

        public Order()
        {

            DeliverCompany = new HashSet<OrderDeliverCompany>();
            RenewOrderdata = new HashSet<RenewOrderdata>();
        }
            
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public int OrderStatus { get; set; }
        public int OrderCase { get; set; }
        public int GenderType { get; set; }
        public int NationalityId { get; set; }
        public int Age { get; set; }
        public string IdentityNumber { get; set; }
        public string IdentityImage { get; set; }
        public string DrivingLicenseImage { get; set; }
        public int RentalPeriod { get; set; }  //مده التاجير
        public DateTime DateRentalPeriod { get; set; }  //تاريخ بدايه الاستجار
        public DateTime DateEndRentalPeriod { get; set; }   //  مده التاجير+تاريخ بدايه الاستجار      (تاريخ نهايه الاستجار
        public int TypePay { get; set; }
        public bool IsPaid { get; set; }
        public string? CoponCode { get; set; }
        public double? coponPrice { get; set; }  // نسبة الخصم
        public double AppPercetage { get; set; }
        public double VatPercetage { get; set; }
        public double AppPrice { get; set; }  // Taken From Provider
        public double VatPrice { get; set; }  // Taken From Client
        public double NetPrice { get; set; }  // (سعر التاجير السياره لليوم الواحد +VatPrice) - coponPrice
        public double  subTotal { get; set; }  //سعر التاجير السياره لليوم الواحد +VatPrice
        public double DiscountValueForDailyRental { get; set; }
        public int BreakPeriod { get; set; } //مده كسر التعاقد  
        public double BreakPeriodPrice { get; set; }
        public int RentalConfirmationDelayPeriod { get; set; }  //المده الفرق بين انهاء الطلب وتاكيد شركه التاجير
        public double RentalConfirmationDelayPrice { get; set; }
        public DateTime? DatePickupCar { get; set; }  //تاريخ استلام السياره
        public DateTime? TimePickupCar { get; set; }  //,وقت استلام السياره
        public string? LocationpickupCar { get; set; }  
        public string? lat { get; set; } 
        public string? lng { get; set; }
        public int? RejectionType { get; set; }  // From Admin Or Rental or Delivery
        public string? ReasonToCancled { get; set; }
        public bool HasRenewToAdditionalPeriod { get; set; }
        public DateTime DateCancelContact { get; set; }  // ده علشان لو تم إلغاء المدة قبل الانتهاء من المده سواء من المستخدم او الاداراة
        public DateTime AdminAprovalDate { get; set; }//تاريخ موافقه الادمن علي الطلب 
        public DateTime RentalCompanyAprovalDate { get; set; }//تاريخ موافقه شركه التاجير علي الطلب 
        public DateTime DeliveryCompanyAprovalDate { get; set; }//تاريخ موافقه شركه التوصيل علي الطلب 
        public bool IsConfirmReciptCar { get; set; }  // تم التاكد علي استلام السياره
        public DateTime DateReceiptCar { get; set; }  // تاريخ استلام السياره من شركه التاجير
        public bool IsCancelContract { get; set; }  // هل تم إلغاء التعاقد 
        public string RentalCompanyId { get; set; }  // شركه التاجير
   
        //Relation
        [InverseProperty(nameof(Mumaken.Domain.Entities.AdditionalTables.OrderCar.Order))]
        public virtual  OrderCar OrderCar { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(ApplicationDbUser.OrderClient))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(RentalCompanyId))]
        [InverseProperty(nameof(ApplicationDbUser.OrdersRenewCompany))]
        public virtual ApplicationDbUser RenewCompany { get; set; }
        [ForeignKey(nameof(NationalityId))]
        public virtual Nationality Nationality { get; set; }

        [InverseProperty(nameof(Mumaken.Domain.Entities.AdditionalTables.OrderDeliverCompany.Order))]
        public ICollection<OrderDeliverCompany> DeliverCompany { get; set; }
        public ICollection <RenewOrderdata> RenewOrderdata { get; set; }

    }
}
