using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.CarTable
{
    public class CarModel:BaseEntity
    {
        public int CarTypeId { get; set; }
        [ForeignKey(nameof(CarTypeId))]
        public virtual CarType CarType { get; set; }
    }
}
