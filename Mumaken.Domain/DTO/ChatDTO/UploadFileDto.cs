using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.ChatDTO
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
    }
}
