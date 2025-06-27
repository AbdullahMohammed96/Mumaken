using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.Location
{
    public class Distract:BaseEntity
    {
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
