using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public interface IRepository
    {
        void Add<T>(T entity);

        void Remove<T>(T entity);

        void Update<T>(T entity, Func<T, bool> predicate);
    }

    public interface IRepositoryGeneric<T>
    {
        void Add(T entity);

        void Remove(T entity);

        void Update(T entity, Func<T, bool> predicate);
    }
}
