using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ConfigCenter.Repository
{
    public interface IRepository<T>
    {
        long Insert(T model);
        int Update(T model, Expression<Func<T, bool>> expression);
        int Update(object updateOnly, Expression<Func<T, bool>> where = null);
        T Get(Expression<Func<T, bool>> expression);
        bool Exists(Expression<Func<T, bool>> expression);

        T Single(Expression<Func<T, bool>> expression);

        List<T> Select(Expression<Func<T, bool>> predicate);

        long Count(Expression<Func<T, bool>> expression);

        int UpdateOnly(Expression<Func<T>> updateFields, Expression<Func<T, bool>> where);

        int UpdateOnly(T obj, Expression<Func<T, object>> onlyFields, Expression<Func<T, bool>> where);

        void InsertAll(IEnumerable<T> items);

        int Delete(Expression<Func<T, bool>> where);
    }
}
