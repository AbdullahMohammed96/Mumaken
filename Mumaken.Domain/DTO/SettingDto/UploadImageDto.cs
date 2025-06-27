using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.SettingDto
{
    public class UploadImageDto
    {
        public IFormFile image { get; set; }
        public int fileName { get; set; }
    }
}
