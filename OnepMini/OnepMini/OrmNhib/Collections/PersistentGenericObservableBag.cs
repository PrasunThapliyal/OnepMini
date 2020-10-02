using NHibernate.Collection.Generic;
using NHibernate.Engine;
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

        public override ICollection GetOrphans(object snapshot, string entityName)
        {
            if (entityName == "OnepMini.OrmNhib.BusinessObjects.OnepValidationresult")
            {
                var orphans = (base.GetOrphans(snapshot, entityName))
                .Cast<T>()
                .ToArray();

                Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphans.Length}");
                if (orphans.Length > 0)
                {
                    Debugger.Break();
                }

                return orphans;
            }
            else if (entityName == "OnepMini.OrmNhib.BusinessObjects.OnepValidochpath")
            {
                var orphans = (base.GetOrphans(snapshot, entityName))
                .Cast<T>()
                .ToArray();

                Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphans.Length}");
                if (orphans.Length > 0)
                {
                    Debugger.Break();
                }

                return orphans;
            }

            var orphanEntities = base.GetOrphans(snapshot, entityName);

            Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphanEntities.Count}");
            if (orphanEntities.Count > 0)
            {
                Debugger.Break();
            }

            return orphanEntities;
        }


        public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName, CancellationToken cancellationToken)
        {
            if (entityName == "OnepMini.OrmNhib.BusinessObjects.OnepValidationresult")
            {
                var orphans = (await base.GetOrphansAsync(snapshot, entityName, cancellationToken))
                .Cast<T>()
                .ToArray();

                Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphans.Length}");
                if (orphans.Length > 0)
                {
                    Debugger.Break();
                }

                return orphans;
            }
            else if (entityName == "OnepMini.OrmNhib.BusinessObjects.OnepValidochpath")
            {
                var orphans = (await base.GetOrphansAsync(snapshot, entityName, cancellationToken))
                .Cast<T>()
                .ToArray();

                Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphans.Length}");
                if (orphans.Length > 0)
                {
                    Debugger.Break();
                }

                return orphans;
            }

            // Eg: entityName = "OnepMini.OrmNhib.BusinessObjects.OnepAmptp"
            // snapshot is the list of AmpTps
            var orphanEntities = await base.GetOrphansAsync(snapshot, entityName, cancellationToken);

            Debug.WriteLine($"Entity: {entityName}, snapshot count: {(snapshot as IList).Count}, orphan count = {orphanEntities.Count}");
            if (orphanEntities.Count > 0)
            {
                Debugger.Break();
            }

            return orphanEntities;
        }
    }
}
