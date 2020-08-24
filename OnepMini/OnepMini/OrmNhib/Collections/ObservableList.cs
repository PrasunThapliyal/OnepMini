
namespace OnepMini.OrmNhib.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NHibernate.Collection;
    using NHibernate.Engine;
    using NHibernate.Persister.Collection;
    using NHibernate.UserTypes;


    public class ObservableList<T> : IUserCollectionType
    {
        public bool Contains(object collection, object entity)
        {
            return ((IList<T>)collection).Contains((T)entity);
        }

        public IEnumerable GetElements(object collection)
        {
            return (IEnumerable)collection;
        }

        public object IndexOf(object collection, object entity)
        {
            return ((IList<T>)collection).IndexOf((T)entity);
        }

        public IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister)
        {
            return new PersistentGenericObservableBag<T>(session);
        }

        public object Instantiate(int anticipatedSize)
        {
            return new List<T>();
        }

        public object ReplaceElements(object original, object target, ICollectionPersister persister, object owner, IDictionary copyCache, ISessionImplementor session)
        {
            IList<T> result = (IList<T>)target;

            result.Clear();
            foreach (object item in ((IEnumerable)original))
            {
                result.Add((T)item);
            }

            return result;
        }

        public IPersistentCollection Wrap(ISessionImplementor session, object collection)
        {
            return new PersistentGenericObservableBag<T>(session, (IList<T>)collection);
        }
    }
}
