namespace Mumaken.Domain.DTO.SettingDTO
{
    public class SettingEditDto
    {
        public int id { get; set; }
        public string phone { get; set; }

        public string condtionsArClient { get; set; }
        public string condtionsEnClient { get; set; }

        public string condtionsArDelegt { get; set; }
        public string condtionsEnDelegt { get; set; }

        public string aboutUsArClient { get; set; }
        public string aboutUsEnClient { get; set; }

        public string aboutUsArDelegt { get; set; }
        public string aboutUsEnDelegt { get; set; }

        public string senderName { get; set; } = "test";
        public string userNameSms { get; set; } = "test";
        public string passwordSms { get; set; } = "test";

        public string applicationId { get; set; }
        public string senderId { get; set; }

        //4jawaly mobily elyamam
        public string smsProvider { get; set; } = "test";
    }
}
