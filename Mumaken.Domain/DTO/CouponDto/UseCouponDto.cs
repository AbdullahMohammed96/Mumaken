using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.CouponDto
{
    public class UseCouponDto
    {
        [Required]
        public string couponCode { get; set; }
        public int couponId { get; set; }
        public string userId { get; set; }
        public double total { get; set; }
        public string lang { get; set; } = "ar";
    }
}
