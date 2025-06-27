using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.Chat;
using Mumaken.Domain.Entities.SettingTables;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.Entities.Location;

namespace Mumaken.Domain.Entities.UserTables
{
    public class ApplicationDbUser : IdentityUser
    {
        public ApplicationDbUser()
        {
            ContactUs = new HashSet<ContactUs>();
            NotifyClient = new HashSet<NotifyUser>();
            DeviceId = new HashSet<DeviceId>();
            Sender = new HashSet<Messages>();
            Receiver = new HashSet<Messages>();
            ConnectUser = new HashSet<ConnectUser>();
            OrderClient = new HashSet<Order>();
            OrdersRenewCompany = new HashSet<Order>();
            Cars = new HashSet<Car>();
        }
        public string PhoneCode { get; set; }
        public bool ActiveCode { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsAppravel { get; set; } = true;    //  Key for Dashboard Not User (if false No Able To Recivie New Order)
        public int Code { get; set; } = 1234;
        public string ShowPassword { get; set; } = "";
        public string Lang { get; set; } = "ar";  //اللغه هتكون عند اليوزر وتكون عربى فى الاول - وتتغير بسيرفس
        public DateTime PublishDate { get; set; } = DateTime.Now;
        //تم اضافته لتعامل مع السيرفس اما 
        //UuserName  ده هنساويه بال email
        public string user_Name { get; set; } //= first_name + " " + last_name;
        public int TypeUser { get; set; } //Client = 1  //deleget = 2 //org_delget = 3//Admin = 4

        public bool CloseNotify { get; set; } = false; //تفعيل الاشعار
        public string ImgProfile { get; set; }
        public string? Lat { get; set; } = "";
        public string? Lng { get; set; } = "";
        public string? Location { get; set; } = "";
        public double Walet { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
        public int? DistrictId { get; set; }
        public int AddedUserType { get; set; }   //Add By Deliver Company Or Not
        public string? CommercialRegisterNumber { get; set; }
        public int? Age { get; set; }
        public int? GenderType { get; set; }
        public int? NantionalityId { get; set; }
        public string? IdentityNumber { get; set; }
        public string? IdentityNumberImage { get; set; }
        public string? DeliveryLicenseImage { get; set; }
        public string? DeliverCompanyId { get; set; }
        //Relation
        [ForeignKey(nameof(DeliverCompanyId))]
        public virtual ApplicationDbUser DeliveryCompany { get; set; }
        [ForeignKey(nameof(NantionalityId))]
        public virtual  Nationality Nationality { get; set; }
        //ContactUs >> user  m>> o
        public virtual ICollection<ContactUs> ContactUs { get; set; }

        //DevieId >> user  m>> o
        public virtual ICollection<DeviceId> DeviceId { get; set; }
        //notifyclient >> user  m>> o
        public virtual ICollection<NotifyUser> NotifyClient { get; set; }
        //notifyDelegt >> user m>> o
        //public virtual ICollection<NotifyDelegt> NotifyDelegt { get; set; }


        // Message
        public virtual ICollection<Messages> Sender { get; set; }
        public virtual ICollection<Messages> Receiver { get; set; }

        public virtual ICollection<ConnectUser> ConnectUser { get; set; }

        // Orders
        public virtual ICollection<Order> OrderClient { get; set; }
        public virtual ICollection<Order> OrdersRenewCompany { get; set; }


        // Cars
        [InverseProperty(nameof(Car.RentalCompany))]
        public virtual ICollection<Car> Cars { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City city { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual Distract Distract { get; set; }
    }
}
