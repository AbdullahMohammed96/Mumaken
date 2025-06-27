using Mumaken.Domain.Common.Helpers.DataTablePaginationServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Common.Helpers
{
    public interface IAppService
    {
        List<T> GetData<T>(PaginationConfiguration outf, IQueryable<T> table, Expression<Func<T, bool>> condition);
    }
}
