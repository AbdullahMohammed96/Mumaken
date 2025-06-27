using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class OrderApiAddDto
    {
        [Required]
        public int servId { get; set; }
        [Required]
        public string providerId { get; set; }
        [Required]
        /// <summary>
        /// 2021-2-20 10:30
        /// </summary>
        public DateTime date { get; set; }
        [Required]
        public int cityId { get; set; }
        [Required]
        public List<IFormFile> img { get; set; }
        [Required]
        public string info { get; set; }
    }
}
