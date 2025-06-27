using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Provider
{
    public class AddProviderViewModel
    {
        public IFormFile ImageProfile  { get; set; }
        public string User_Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCode { get; set; }
        public int CityId { get; set; }
        public int DistractId { get; set; }
        public string Location { get; set; }
        public string Lat { get; set; }
        public string  Lng{ get; set; }
        public string CommercialRegisterNumber { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string? Lang { get; set; }
    }
}
