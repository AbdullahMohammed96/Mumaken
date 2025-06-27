using System;
using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
        public string ChangeLangName(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, NameAr, NameEn);
        }
    }
}
