using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Home
{
    public class RentalCompanyIndexViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgProfile { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string CommercialRegisterNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
