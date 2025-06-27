using System;

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class UserInfoAddDTO
    {
        public string userName { get; set; }
        public string companyName { get; set; }
        public string email { get; set; }
        public string location { get; set; }
        public string phone { get; set; }
        public int code { get; set; }
        public string password { get; set; }
        public string deviceId { get; set; }
        public string deviceType { get; set; }
        /// <summary>
        /// for title of notification
        /// </summary>
        public string projectName { get; set; }

        /// <summary>
        ///ar or en
        /// </summary>

        public string lang { get; set; } = "ar";
    }
}
