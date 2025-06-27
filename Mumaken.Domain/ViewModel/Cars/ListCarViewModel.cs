using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Cars
{
    public class ListCarViewModel
    {
        public int Id { get; set; }
        public string CarNumber { get; set; }
        public string Image { get; set; }
        public string CarModel { get; set; }
        public string CarType { get; set; }
        public string CarCategory { get; set; }
        public double DailyPrice { get; set; }
    }
}
