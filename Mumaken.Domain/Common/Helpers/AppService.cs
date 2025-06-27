using Mumaken.Domain.Common.Helpers.DataTablePaginationServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Common.Helpers
{
    public class AppService : IAppService
    {


            public List<T> GetData<T>(PaginationConfiguration outf, IQueryable<T> customerData, Expression<Func<T, bool>> condition)
        {

            //Sorting
            //if (!(string.IsNullOrEmpty(outf.sortColumn) && string.IsNullOrEmpty(outf.sortColumnDirection)))
            //{
            //    //customerData = customerData.OrderBy(o => outf.sortColumn + " " + outf.sortColumnDirection);
            //    customerData = customerData.OrderBy(o => outf.sortColumn);
            //}
            var entityType = typeof(T);
            if (!string.IsNullOrEmpty(outf.sortColumn))
            {
                if (outf.sortColumn == "typeUserText")
                {
                    outf.sortColumn = "typeUser";
                }

                if (outf.sortColumn == "city")
                {
                    outf.sortColumn = "cityId";
                }

                var property = entityType.GetProperty(outf.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {

                    outf.sortColumnDirection = outf.sortColumn == "creationDate" ? "desc" : outf.sortColumnDirection;

                    var parameter = Expression.Parameter(entityType, "x");
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                    var orderByMethod = outf.sortColumnDirection == "asc" ? "OrderBy" : "OrderByDescending";
                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        orderByMethod,
                        new Type[] { entityType, property.PropertyType },
                        customerData.Expression,
                        Expression.Quote(orderByExpression));
                    customerData = customerData.Provider.CreateQuery<T>(resultExpression);

                }
            }
            //Search
            if (!string.IsNullOrEmpty(outf.searchValue))
            {
                customerData = customerData.Where(condition);
            }

            //total number of rows count 
            outf.recordsTotal = customerData.Count();
            //Paging 

            //var data = customerData.OrderBy(o => outf.sortColumn + " " + outf.sortColumnDirection);
            var data = customerData.Skip(outf.skip).Take(outf.pageSize).ToList();


            return data;
        }


    }
}
