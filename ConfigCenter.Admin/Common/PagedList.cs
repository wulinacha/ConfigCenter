using System;
using System.Collections;
using System.Collections.Generic;

namespace ConfigCenter.Admin.Common
{
    /// <summary>
    /// 包含分页信息的集合列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T>: IEnumerable<T>, IEnumerable
    {
        #region ctor
        public PagedList(IList<T> list,int pageIndex, int pageSize,int total)
        {
            this.List = list;
            this.Total = total;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
        public PagedList(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        #endregion
        public int StartItemIndex { get; set; } = 0;
        /// <summary>
        /// 数据记录
        /// </summary>
        public IList<T> List { get; set; }


        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        public long Total { get; set; }


        /// <summary>
        /// 页面总数，设置了PageSize和RecordTotal属性后自动计算
        /// </summary>
        private int _dataCount;
        public int DataCount
        {
            get
            {
                if (PageSize == 0 || Total == 0) return 0;

                return (Total / PageSize).ToInt() + (Total % PageSize > 0 ? 1 : 0);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.List.GetEnumerator();
        }
    }

    public static class Extensions
    {

        /// <summary>
        /// object转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <param name="def">默认值为0，可选</param>
        /// <returns></returns>
        public static int ToInt(this object obj, int def = 0)
        {
            if (obj == null) return def;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                int result = 0;
                if (int.TryParse(obj.ToString(), out result)) return result;
                else return def;
            }
        }
    }
 }