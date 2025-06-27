using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Common.Helpers.DataTablePaginationServer
{
    public class PaginationConfiguration
    {
        public string draw { get; set; }
        public string start { get; set; }
        public string length { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }
        public string searchValue { get; set; }

        public int pageSize { get; set; }
        public int skip { get; set; }
        public int recordsTotal { get; set; }
    }
}
