using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace testing_with_nunit
{
    public class FakeDbSet<T> : IDbSet<T> where T : class, new()
    {
        public FakeDbSet(List<T> entries)
        {
            _collections = entries;
        }
        Type IQueryable.ElementType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        Expression IQueryable.Expression
        {
            get
            {
                return _collections.AsQueryable().Expression;
            }
        }

        List<T> _collections;

        ObservableCollection<T> IDbSet<T>.Local
        {
            get
            {
                return new ObservableCollection<T>(_collections);
            }
        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                return _collections.AsQueryable().Provider;
            }
        }

        T IDbSet<T>.Add(T entity)
        {
            _collections.Add(entity);
            return entity;
        }

        T IDbSet<T>.Attach(T entity)
        {
            return entity;
        }

        T IDbSet<T>.Create()
        {
            return new T();
        }

        TDerivedEntity IDbSet<T>.Create<TDerivedEntity>()
        {
            return null;
        }

        T IDbSet<T>.Find(params object[] keyValues)
        {
            return new T();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collections.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _collections.GetEnumerator();
        }

        T IDbSet<T>.Remove(T entity)
        {
            _collections.Remove(entity);
            return entity; ;
        }
    }
}
