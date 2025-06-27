using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.UserIndexDto
{
    public class UserIndexDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImgProfile { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public int TypeUser { get; set; }
        public string TypeUserText { get; set; } // Add By Driver Company Or Not
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
