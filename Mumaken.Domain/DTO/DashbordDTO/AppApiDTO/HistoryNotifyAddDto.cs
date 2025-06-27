using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.DashbordDTO.AppDTO
{
    public class HistoryNotifyAddApiDto
    {
        public string Text { get; set; }
        public List<string> UserId { get; set; }
    }
}
