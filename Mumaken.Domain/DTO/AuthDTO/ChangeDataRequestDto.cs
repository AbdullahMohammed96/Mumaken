using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class ChangeDataRequestDto
    {
        public string? UserName { get; set; } = null;
        public string? Email { get; set; } = null;
        public IFormFile? Image { get; set; } = null;
        public string Lang { get; set; }

    }
}
