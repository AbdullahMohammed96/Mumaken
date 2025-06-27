using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Provider
{
    public  class UpadateProviderViewModel
    {
        public string id { get; set; }
        public IFormFile imageProfile { get; set; }
        public string userName { get; set; }
        public int cityId { get; set; }
        public int distractId { get; set; }
        public string address { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string commercialRegisterNumber { get; set; }
        public string lang { get; set; }
    }
}
