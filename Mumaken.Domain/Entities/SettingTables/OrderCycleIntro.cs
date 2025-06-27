using AAITHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.SettingTables
{
    public class OrderCycleIntro
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public string ChangeLangTitle(string lang = "ar") => HelperMsg.creatMessage(lang, TitleAr, TitleEn);
        public string ChangeLangDescription(string lang = "ar") => HelperMsg.creatMessage(lang, DescriptionAr, DescriptionEn);
    }
}
