using Mumaken.Domain.Entities.SettingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Persistence.Seeds
{
    public static class DefaultSettings
    {
        public static Setting BasicSettingAsync()
        {

            return new Setting
            {
                Id = 1,
                Phone = "0123456789",
                CondtionsArClient = "الشروط والاحكام عميل بالعربية",
                CondtionsEnClient = "CondtionsEnClient",
                AboutUsArClient = "عن التطبيق عميل بالعربية",
                AboutUsEnClient = "AboutUsEnClient",

                CondtionsArProvider = "الشروط والاحكام مقدم خدمة بالعربية",
                CondtionsEnProvider = "CondtionsEnDelegt",
                AboutUsArProvider = "عن التطبيق مقدم خدمة بالعربية",
                AboutUsEnProvider = "AboutUsEnDelegt",

                ApplicationId = "test",
                SenderId = "test",
                PasswordSms = "test",
                SenderName = "test",
                UserNameSms = "test",
                SmsProvider = "test",
                Address = "test location",
                Email = "test@mail.com",
                Lat = "12.22541",
                Lng = "14.11547"


            };



        }
    }
}
