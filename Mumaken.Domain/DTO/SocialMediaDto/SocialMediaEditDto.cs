using Microsoft.AspNetCore.Http;

namespace Mumaken.Domain.DTO.SocialMediaDTO
{
    public class SocialMediaEditDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public IFormFile img { get; set; }
        public string url { get; set; }
    }
}
