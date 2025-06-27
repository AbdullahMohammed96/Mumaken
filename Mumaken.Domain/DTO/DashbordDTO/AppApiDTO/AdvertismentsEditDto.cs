using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.DashbordDTO.AppDTO
{
    public class AdvertismentsEditApiDto
    {
        public int Id { get; set; }
        public string TitelAr { get; set; }
        public string TitelEn { get; set; }
        public IFormFile ImgFile { get; set; }
    }
}
