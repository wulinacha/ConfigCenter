using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace ConfigCenter.Admin.Common
{
    public static class PageListExtensions
    {
        public static Webdiyer.AspNetCore.PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize,long TotalCount)
        {
            var pagedList= allItems.ToPagedList(pageIndex,pageSize);
            pagedList.TotalItemCount = (int)TotalCount;
            pagedList.CurrentPageIndex = pageIndex;

            return pagedList;
        }
        //public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize);
        //public static PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize);
    }
}
