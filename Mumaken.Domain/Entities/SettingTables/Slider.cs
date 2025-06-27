using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.SettingTables
{
    public class Slider
    {
        public int Id { get; set; }
        public string ImageAr { get; set; }
        public string ImageEn { get; set; }
        public bool IsActive { get; set; }
    }
}
