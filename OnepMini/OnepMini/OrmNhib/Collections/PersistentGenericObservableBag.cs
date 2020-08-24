using NHibernate.Collection.Generic;
using NHibernate.Engine;
using OnepMini.OrmNhib.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnepMini.OrmNhib.Collections
{
    public class PersistentGenericObservableBag<T> : PersistentGenericBag<T>, IList<T>
    {
        public PersistentGenericObservableBag(ISessionImplementor session)
            :base(session)
        {

        }

        public PersistentGenericObservableBag(ISessionImplementor session, ICollection<T> original)
        : base(session, original)
        {
        }


        public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName, CancellationToken cancellationToken)
        {
            if (entityName == "OnepMini.OrmNhib.BusinessObjects.OnepValidationresult")
            {
                var orphans = (await base.GetOrphansAsync(snapshot, entityName, cancellationToken))
                .Cast<T>()
                .Where(b => ReferenceEquals(null, (b as OnepValidationresult).OnepNetwork))
                .ToArray();

                Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphans.Length}");

                return orphans;
            }
            else if (entityName == "OnepMini.OrmNhib.BusinessObjects.OnepValidochpath")
            {
                var orphans = (await base.GetOrphansAsync(snapshot, entityName, cancellationToken))
                .Cast<T>()
                .Where(b => ReferenceEquals(null, (b as OnepValidochpath).OnepNetwork))
                .ToArray();

                Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphans.Length}");

                return orphans;
            }

            // Eg: entityName = "OnepMini.OrmNhib.BusinessObjects.OnepAmptp"
            // snapshot is the list of AmpTps
            var orphanEntities = await base.GetOrphansAsync(snapshot, entityName, cancellationToken);

            Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphanEntities.Count}");

            return orphanEntities;
        }
    }
}
