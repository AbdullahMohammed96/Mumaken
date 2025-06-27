using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Common.Helpers.Extensions
{
    public static class Extensions
    {
        public static async Task<Pagination<T>> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            try
            {
                var totalItems = query.Count();

                Pagination<T> pagination = new Pagination<T>
                {
                    TotalItems = totalItems,
                    PageSize = pageSize,
                    CurrentPage = pageNumber
                };

                pagination.Result = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return pagination;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
