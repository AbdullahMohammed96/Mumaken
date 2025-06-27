using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Common.Helpers.DataTablePaginationServer
{
    public static class Pagination
    {
        public static PaginationConfiguration OutFun(this HttpRequest hr)
        {
            PaginationConfiguration re = new PaginationConfiguration();

            var draw = hr.Form["draw"].FirstOrDefault();
            // Skiping number of Rows count
            var start = hr.Form["start"].FirstOrDefault();
            // Paging Length 10,20
            var length = hr.Form["length"].FirstOrDefault();
            // Sort Column Name
            var sortColumn = hr.Form["columns[" + hr.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)
            var sortColumnDirection = hr.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)
            var searchValue = hr.Form["search[value]"].FirstOrDefault();

            //Paging Size (10,20,50,100)
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;


            re.draw = draw;
            re.start = start;
            re.length = length;
            re.sortColumn = sortColumn;
            re.sortColumnDirection = sortColumnDirection;
            re.searchValue = searchValue;
            re.pageSize = pageSize;
            re.skip = skip;
            re.recordsTotal = recordsTotal;


            return re;


        }
    }
}
