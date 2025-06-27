using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class UpdateDataUserDto
    {
     

        public string lang { get; set; } = "ar";
        public string? userName { get; set; }
        public DateTime birhDate { get; set; }
        public IFormFile? imgProfile { get; set; }
        public int? cityId { get; set; }

    }
}
