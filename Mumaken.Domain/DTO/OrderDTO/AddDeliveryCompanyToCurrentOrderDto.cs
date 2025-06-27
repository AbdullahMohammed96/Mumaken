using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class AddDeliveryCompanyToCurrentOrderDto
    {
        public int OrderId { get; set; }
        public string lang { get; set; }
        public List<CompanyToCurrentInfoDto> workedCompany { get; set; }  // شركات التوصيل التي تعمل معها فعلا
        public List<string> newCompanyWantedWorkId { get; set; }  // شركات التوصيل التي تريد ان تعمل معها
    }
   
    public class CompanyToCurrentInfoDto
    {
        public string deliverCompanyId { get; set; }
        public string loginData { get; set; }
    }
}
