using Mumaken.Domain.DTO.CategoryDto;
using System;
using System.Collections.Generic;

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class UserInfoDTO: BaseInfoDto
    {
     
    }
    public class BaseInfoDto
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string phoneCode { get; set; }
        public string lang { get; set; }
        public bool closeNotify { get; set; }
        public bool status { get; set; }
        public string imgProfile { get; set; }
        public string token { get; set; }
        public int typeUser { get; set; }
        public int code { get; set; }
        public bool ActiveCode { get; set; }
        public string city { get; set; }
        public int cityId { get; set; }
        public int permissiblePeriod { get; set; }
        public int age { get; set; }
        public string birthDate { get; set; }
        public string registerDate { get; set; }
    }
}
