using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace ConfigCenter.Repository.Implement
{
    public class OrmliteRepository<T> : IRepository<T>//where T: IAggregateRoot
    {
        private readonly IDbConnection _db;
        public OrmliteRepository()
        {
            this._db = Database.ormLiteConnectionFactory.CreateDbConnection();
        }

        internal IDbTransaction _tran=null;

        public IDbTransaction BeginTransaction(IsolationLevel iso)
        {
            _tran = _db.BeginTransaction();
            return _tran;
        }

        public void Commet() {
            _tran.Commit();
        }

        public void Rollback()
        {
            _tran.Rollback();
        }


        public long Insert(T model)
        {
            return _db.Insert(model);
        }

        public int Update(T model, Expression<Func<T, bool>> expression)
        {
            return _db.Update<T>(model, expression);
        }

        public List<T> GetList(string sql)
        {
            return _db.Select<T>(sql);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _db.Single(expression);
        }
        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _db.Exists(expression);
        }

        public int Update(object updateOnly, Expression<Func<T, bool>> where = null)
        {
            return _db.Update(updateOnly, where);
        }

        public T Single(Expression<Func<T, bool>> expression)
        {
            return _db.Single(expression);
        }

        public List<T> Select(Expression<Func<T, bool>> predicate)
        {
            return _db.Select(predicate);
        }

        public long Count(Expression<Func<T, bool>> expression)
        {
            return _db.Count(expression);
        }

        public int UpdateOnly(Expression<Func<T>> updateFields, Expression<Func<T, bool>> where)
        {
            return _db.UpdateOnly(updateFields, where);
        }

        public int UpdateOnly(T obj, Expression<Func<T, object>> onlyFields, Expression<Func<T, bool>> where)
        {
            return _db.UpdateOnly(obj, onlyFields, where);
        }

        public void InsertAll(IEnumerable<T> items)
        {
            items.Each(item =>
            {
                _db.Insert(item);
            });
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            return _db.Delete(where);
        }
       
    }
}
