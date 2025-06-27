using Mumaken.Domain.Common.Helpers.DataTablePaginationServer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.DTO.SettingDto;

namespace Mumaken.Domain.Common.Helpers
{
    public interface IHelper
    {
        public string Upload(IFormFile Photo, int FileName);
        Task<string> UploadFileUsingApi(UploadImageDto model);
        public string GetRole(string role, string lang);
        public string CreatePDF(string controllerAction, int id);
        string GenerateToken(ApplicationDbUser user);
        string ValidateImage(IFormFile? image);
    }
}
