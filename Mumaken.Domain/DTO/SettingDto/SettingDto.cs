namespace Mumaken.Domain.DTO.SettingDTO
{
    public class SettingDto
    {
        public string phone { get; set; }
        public string email { get; set; }
        public string aboutUs { get; set; }
        public string condtions { get; set; }
        public string condtionsProvider { get; set; }
        public string aboutUsProvider { get; set; }

        public string lat { get; set; }
        public string lng { get; set; }
        public string location { get; set; }

        //public string senderName { get; set; } = "test";
        //public string userNameSms { get; set; } = "test";
        //public string passwordSms { get; set; } = "test";

        //public string applicationId { get; set; }
        //public string senderId { get; set; }

        ////4jawaly mobily elyamam
        //public string smsProvider { get; set; } = "test";
    }
}
