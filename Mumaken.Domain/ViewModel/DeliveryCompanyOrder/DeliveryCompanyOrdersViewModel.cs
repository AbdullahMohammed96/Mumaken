﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.DeliveryCompanyOrder
{
    public class DeliveryCompanyOrdersViewModel
    {
        public int OrderId { get; set; }
        public string CaptionName { get; set; }
        public string CaptionPhone { get; set; }
        public string CreationDate { get; set; }
        public string OrderType { get; set; }
    }
}
