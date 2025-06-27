using Mumaken.Domain.DTO.LogicClientDto.PackageDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.LogicClientDto.SubscriptionDTO
{
    public class SubscriptionDto
    {
        public string PackageName { get; set; }
        public string startDate { get; set; }
        public bool IsFreePackage { get; set; }
        public string EndDate { get; set; }
        public List<PackageListDto> Packages { get; set; }
    }
}
