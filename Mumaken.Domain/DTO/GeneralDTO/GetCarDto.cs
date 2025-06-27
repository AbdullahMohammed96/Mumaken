using Mumaken.Domain.Common.Helpers.ResponseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.GeneralDTO
{
    public class GetCarDto
    {
        public int Id { get; set; }
        public string carCategory { get; set; }
        public string carType { get; set; }
        public string carModel { get; set; }
        public string note { get; set; }
        public string image { get; set; }
        public double dailyRentalPrice { get; set; }

        public CarRentalInformationDto CarRentalInfo { get; set; }
    }
    public class CarRentalInformationDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

        public string image { get; set; }
        public string commercialRegisterNumber { get; set; }
    }
    public class GetAllCarResponseWithSomeSettingInformatio
    {
        public Pagination<GetCarDto> cars { get; set; }
        public string textRenewAndCancelAr { get; set; }
        public double vatPercetage { get; set; }
    }
}
