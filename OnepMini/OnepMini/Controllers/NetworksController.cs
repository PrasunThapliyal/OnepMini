
namespace OnepMini.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using NHibernate;
    using OnepMini.OrmNhib.BusinessObjects;
    using OnepMini.OrmNhib.Initializer;

    [Route("api/[controller]")]
    [ApiController]
    public class NetworksController : ControllerBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;
        private readonly ISessionFactory _sessionFactory;
        private readonly NHibernate.Cfg.Configuration _configuration;

        public NetworksController(
            INHibernateInitializer nHibernateInitializer,
            ISessionFactory sessionFactory)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));

            _configuration = _nHibernateInitializer.GetConfiguration();
        }
        // GET: api/Networks
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //return new string[] { "value1", "value2" };
            List<string> tableEntries = new List<string>();

            var session = _sessionFactory.OpenSession();
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    int rowCount = 0;
                    foreach (var classMapping in _configuration.ClassMappings)
                    {
                        var results = session.CreateCriteria(classMapping.EntityName).List();
                        foreach (var result in results)
                        {
                            string s = classMapping.EntityName + ": " + result + " oid = " + (result as BusinessBase<long>).Id;
                            tableEntries.Add(s);
                            rowCount++;
                        }
                    }

                    tableEntries.Add($"Row Count = {rowCount}");
                    tx.Commit();
                    session.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();

                    throw;
                }
            }

            return tableEntries;
        }

        // GET: api/Networks/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Networks
        [HttpPost]
        public async Task<object> Post([FromBody] string value)
        {
            // Create a basic Onep Network and save to DB
            OnepNetwork onepNetwork = CreateNewOnepNetwork();

            var session = _sessionFactory.OpenSession();
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    await session.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();

                    throw;
                }
            }
            return new { Id = onepNetwork.Id };
        }

        private static OnepNetwork CreateNewOnepNetwork()
        {
            OnepNetwork onepNetwork;
            {
                onepNetwork = new OnepNetwork()
                {
                    McpProjectId = Guid.NewGuid().ToString(),
                    Name = "Test 01"
                };
                var onepFiberTL1 = new OnepFibertl()
                {
                    Name = "FS01_1",
                    Length = 30,
                    Loss = 30 * 0.25,
                    OnepNetwork = onepNetwork
                };
                var onepFiberTL2 = new OnepFibertl()
                {
                    Name = "FS01_2",
                    Length = 30,
                    Loss = 30 * 0.25,
                    OnepNetwork = onepNetwork
                };
                onepFiberTL1.OnepTopologicallinkMemberByUniMate = onepFiberTL2;
                onepFiberTL2.OnepTopologicallinkMemberByUniMate = onepFiberTL1;

                onepNetwork.OnepFibertls.Add(onepFiberTL1);
                onepNetwork.OnepFibertls.Add(onepFiberTL2);

                /////////

                var onepTP1 = new OnepTerminationpoint()
                {
                    Name = "TP 01",
                    Ptp = 1,
                    Role = 2,
                    OnepNetwork = onepNetwork
                };
                var onepTP2 = new OnepTerminationpoint()
                {
                    Name = "TP 02",
                    Ptp = 1,
                    Role = 2,
                    OnepNetwork = onepNetwork
                };

                onepNetwork.OnepTerminationpoints.Add(onepTP1);
                onepNetwork.OnepTerminationpoints.Add(onepTP2);

                onepTP1.OnepTopologicallinksForAEndTP.Add(onepFiberTL1);
                onepTP1.OnepTopologicallinksForZEndTP.Add(onepFiberTL2);
                onepFiberTL1.OnepTerminationpointByAEndTP = onepTP1;
                onepFiberTL2.OnepTerminationpointByAEndTP = onepTP2;

                onepTP2.OnepTopologicallinksForAEndTP.Add(onepFiberTL2);
                onepTP2.OnepTopologicallinksForZEndTP.Add(onepFiberTL1);
                onepFiberTL1.OnepTerminationpointByZEndTP = onepTP2;
                onepFiberTL2.OnepTerminationpointByZEndTP = onepTP1;

                var onepAmpTP1 = new OnepAmptp()
                {
                    TargetGain = 2.1,
                    OnepNetwork = onepNetwork,
                    OnepTerminationpoint = onepTP1
                };
                onepTP1.OnepAmpRole = onepAmpTP1;
                onepNetwork.OnepAmptps.Add(onepAmpTP1);

                var onepAmpTP2 = new OnepAmptp()
                {
                    TargetGain = 1.9,
                    OnepNetwork = onepNetwork,
                    OnepTerminationpoint = onepTP2
                };
                onepTP2.OnepAmpRole = onepAmpTP2;
                onepNetwork.OnepAmptps.Add(onepAmpTP2);

                var onepValidationResult01 = new OnepValidationresult
                {
                    Status = 1,
                    OnepNetwork = onepNetwork
                };

                var onepVP01_1 = new OnepValidochpath
                {
                    Name = "VP 01_1",
                    OnepNetwork = onepNetwork,
                    Pmd = 2.3,
                    OnepValidationresult = onepValidationResult01
                };
                onepNetwork.OnepValidochpaths.Add(onepVP01_1);

                var onepVP01_2 = new OnepValidochpath
                {
                    Name = "VP 01_2",
                    OnepNetwork = onepNetwork,
                    Pmd = 2.4,
                    OnepValidationresult = onepValidationResult01
                };
                onepNetwork.OnepValidochpaths.Add(onepVP01_2);

                onepValidationResult01.OnepValidochpaths.Add(onepVP01_1);
                onepValidationResult01.OnepValidochpaths.Add(onepVP01_2);
                onepNetwork.OnepValidationresults.Add(onepValidationResult01);

                // Create another Validation result

                var onepValidationResult02 = new OnepValidationresult
                {
                    Status = 1,
                    OnepNetwork = onepNetwork
                };
                onepNetwork.OnepValidationresults.Add(onepValidationResult02);

            }

            return onepNetwork;
        }

        // POST: api/Networks
        [HttpPost("CreateNetworkForLEReset_01")]
        public async Task<object> CreateNetworkForLEReset_01([FromBody] string value)
        {
            // Create a basic Onep Network and save to DB
            OnepNetwork onepNetwork = CreateNetworkForLEReset_01();

            var session = _sessionFactory.OpenSession();
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    await session.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();

                    throw;
                }
            }
            return new { Id = onepNetwork.Id };
        }

        [HttpPut("EditNetwork/{id}/ResetLE_01")]
        public async Task<object> ResetLE_01(long id, [FromBody] string value)
        {
            // This sequence of changes results in the following exception:
            // NHibernate.ObjectDeletedException: deleted object would be re-saved by cascade (remove deleted object from associations)[OnepMini.OrmNhib.BusinessObjects.OnepValidochpath#163840]

            // Summary:
            // We already have VR01 and VR02 in the network
            // During LE reset, move VPs from under VR01 to VR02
            // And then we remove VR01 from network

            // Get the network
            var session = _sessionFactory.OpenSession();
            var onepNetwork = session.Get<OnepNetwork>(id);

            // LE Reset
            // Get ValidationResult01's VP's and associate them with a new ValidationResult
            ResetLE_01(onepNetwork);

            // Finally save the network back
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    await session.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();
                    throw;
                }
            }

            return new { Id = id };

        }

        private static OnepNetwork CreateNetworkForLEReset_01()
        {
            // This network and its corresponding LE Reset is causing
            // Deleted object would be re-saved exception

            OnepNetwork onepNetwork;
            {
                onepNetwork = new OnepNetwork()
                {
                    McpProjectId = Guid.NewGuid().ToString(),
                    Name = "Test 01"
                };
                var onepFiberTL1 = new OnepFibertl()
                {
                    Name = "FS01_1",
                    Length = 30,
                    Loss = 30 * 0.25,
                    OnepNetwork = onepNetwork
                };
                var onepFiberTL2 = new OnepFibertl()
                {
                    Name = "FS01_2",
                    Length = 30,
                    Loss = 30 * 0.25,
                    OnepNetwork = onepNetwork
                };
                onepFiberTL1.OnepTopologicallinkMemberByUniMate = onepFiberTL2;
                onepFiberTL2.OnepTopologicallinkMemberByUniMate = onepFiberTL1;

                onepNetwork.OnepFibertls.Add(onepFiberTL1);
                onepNetwork.OnepFibertls.Add(onepFiberTL2);

                /////////

                var onepTP1 = new OnepTerminationpoint()
                {
                    Name = "TP 01",
                    Ptp = 1,
                    Role = 2,
                    OnepNetwork = onepNetwork
                };
                var onepTP2 = new OnepTerminationpoint()
                {
                    Name = "TP 02",
                    Ptp = 1,
                    Role = 2,
                    OnepNetwork = onepNetwork
                };

                onepNetwork.OnepTerminationpoints.Add(onepTP1);
                onepNetwork.OnepTerminationpoints.Add(onepTP2);

                onepTP1.OnepTopologicallinksForAEndTP.Add(onepFiberTL1);
                onepTP1.OnepTopologicallinksForZEndTP.Add(onepFiberTL2);
                onepFiberTL1.OnepTerminationpointByAEndTP = onepTP1;
                onepFiberTL2.OnepTerminationpointByAEndTP = onepTP2;

                onepTP2.OnepTopologicallinksForAEndTP.Add(onepFiberTL2);
                onepTP2.OnepTopologicallinksForZEndTP.Add(onepFiberTL1);
                onepFiberTL1.OnepTerminationpointByZEndTP = onepTP2;
                onepFiberTL2.OnepTerminationpointByZEndTP = onepTP1;

                var onepAmpTP1 = new OnepAmptp()
                {
                    TargetGain = 2.1,
                    OnepNetwork = onepNetwork,
                    OnepTerminationpoint = onepTP1
                };
                onepTP1.OnepAmpRole = onepAmpTP1;
                onepNetwork.OnepAmptps.Add(onepAmpTP1);

                var onepAmpTP2 = new OnepAmptp()
                {
                    TargetGain = 1.9,
                    OnepNetwork = onepNetwork,
                    OnepTerminationpoint = onepTP2
                };
                onepTP2.OnepAmpRole = onepAmpTP2;
                onepNetwork.OnepAmptps.Add(onepAmpTP2);

                var onepValidationResult01 = new OnepValidationresult
                {
                    Status = 1,
                    OnepNetwork = onepNetwork
                };

                var onepVP01_1 = new OnepValidochpath
                {
                    Name = "VP 01_1",
                    OnepNetwork = onepNetwork,
                    Pmd = 2.3,
                    OnepValidationresult = onepValidationResult01
                };
                onepNetwork.OnepValidochpaths.Add(onepVP01_1);

                var onepVP01_2 = new OnepValidochpath
                {
                    Name = "VP 01_2",
                    OnepNetwork = onepNetwork,
                    Pmd = 2.4,
                    OnepValidationresult = onepValidationResult01
                };
                onepNetwork.OnepValidochpaths.Add(onepVP01_2);

                onepValidationResult01.OnepValidochpaths.Add(onepVP01_1);
                onepValidationResult01.OnepValidochpaths.Add(onepVP01_2);
                onepNetwork.OnepValidationresults.Add(onepValidationResult01);

                // Create another Validation result

                var onepValidationResult02 = new OnepValidationresult
                {
                    Status = 1,
                    OnepNetwork = onepNetwork
                };
                onepNetwork.OnepValidationresults.Add(onepValidationResult02);

            }

            return onepNetwork;
        }

        private void ResetLE_01(OnepNetwork onepNetwork)
        {
            // Network has 1 ValidationResult which has 2 VPs attached

            // We want to try re-parenting VPs, i.e. Remove ValidationResult01 and Create ValidationResult02
            // And move the existing VPs to the new ValidationResult

            var onepValidationresult01 = onepNetwork.OnepValidationresults[0];
            var oldVPs = onepValidationresult01.OnepValidochpaths;

            //OnepValidationresult onepValidationresult02 = new OnepValidationresult
            //{
            //    OnepNetwork = onepNetwork,
            //    OnepValidochpaths = oldVPs
            //};
            var onepValidationresult02 = onepNetwork.OnepValidationresults[1];
            foreach (var vp in oldVPs)
            {
                onepValidationresult02.OnepValidochpaths.Add(vp);
                vp.OnepValidationresult = onepValidationresult02;
            }

            onepNetwork.OnepValidationresults.Remove(onepValidationresult01);
            onepValidationresult01.OnepNetwork = null;
            onepValidationresult01.OnepValidochpaths.Clear();

            onepNetwork.OnepValidationresults.Add(onepValidationresult02);
        }

        // PUT: api/Networks/5
        [HttpPut("EditNetwork/{id}/ChangeTerminationPoint")]
        public async Task<object> ChangeTerminationPoint(long id, [FromBody] string value)
        {
            var session = _sessionFactory.OpenSession();
            var onepNetwork = session.Get<OnepNetwork>(id);
            using (var tx = session.BeginTransaction())
            {
                try
                {

                    // Replace TP01 by new TP03, but move the same AmpRole object to TP03
                    EditNetworkChangeTerminationPoint(onepNetwork);

                    await session.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();

                    throw;
                }
            }
            return new { Id = id };

        }

        private static void EditNetworkChangeTerminationPoint(OnepNetwork onepNetwork_x)
        {
            // Replace TP01 by new TP03, but move the same AmpRole object to TP03

            var onepTP3 = new OnepTerminationpoint()
            {
                Name = "TP 03",
                Ptp = 1,
                Role = 2,
                OnepNetwork = onepNetwork_x
            };

            var onepTP1 = onepNetwork_x.OnepTerminationpoints.Where(p => p.Name == "TP 01").FirstOrDefault();
            onepNetwork_x.OnepTerminationpoints.Remove(onepTP1);
            onepNetwork_x.OnepTerminationpoints.Add(onepTP3);
            var onepFiberTL1 = onepNetwork_x.OnepFibertls.Where(p => p.Name == "FS01_1").FirstOrDefault();
            var onepFiberTL2 = onepNetwork_x.OnepFibertls.Where(p => p.Name == "FS01_2").FirstOrDefault();
            onepFiberTL1.OnepTerminationpointByAEndTP = null;
            onepFiberTL2.OnepTerminationpointByZEndTP = null;

            onepTP3.OnepTopologicallinksForAEndTP.Add(onepFiberTL1);
            onepTP3.OnepTopologicallinksForZEndTP.Add(onepFiberTL2);
            onepFiberTL1.OnepTerminationpointByAEndTP = onepTP3;
            onepFiberTL2.OnepTerminationpointByZEndTP = onepTP3;

            var onepAmpTP1 = onepTP1.OnepAmpRole;
            onepTP3.OnepAmpRole = onepAmpTP1;

            //onepTP1.OnepAmpRole = null; // This is not required
            //onepTP1.OnepNetwork = null;
            //onepTP1.OnepTopologicallinksForAEndTP.Clear();
            onepTP1.OnepTopologicallinksForAEndTP = null;

            //onepTP1.OnepTopologicallinksForZEndTP.Clear();
            onepTP1.OnepTopologicallinksForZEndTP = null;
        }


        [HttpPut("EditNetwork/{id}/EditVPs")]
        public async Task<object> EditVPs(long id, [FromBody] string value)
        {
            // Get the network
            var session = _sessionFactory.OpenSession();
            var onepNetwork = session.Get<OnepNetwork>(id);

            // LE Reset
            // Get ValidationResult01's VP's and associate them with a new ValidationResult
            EditNetworkEditVPs(onepNetwork);

            // Finally save the network back
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    await session.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();

                    throw;
                }
            }
            return new { Id = id };

        }

        private void EditNetworkEditVPs(OnepNetwork onepNetwork)
        {
            // Network has 1 ValidationResult which has 2 VPs attached

            // We want to try re-parenting VPs, i.e. Remove ValidationResult01 and Create ValidationResult02
            // And move the existing VPs to the new ValidationResult

            var onepValidationresult01 = onepNetwork.OnepValidationresults[0];
            var oldVPs = onepValidationresult01.OnepValidochpaths;
            //onepValidationresult01.OnepValidochpaths.Clear();

            //OnepValidationresult onepValidationresult02 = new OnepValidationresult
            //{
            //    OnepNetwork = onepNetwork,
            //    OnepValidochpaths = oldVPs
            //};
            var onepValidationresult02 = onepNetwork.OnepValidationresults[1];
            foreach (var vp in oldVPs)
            {
                onepValidationresult02.OnepValidochpaths.Add(vp);
                vp.OnepValidationresult = onepValidationresult02;
            }

            onepNetwork.OnepValidationresults.Remove(onepValidationresult01);
            //onepNetwork.OnepValidationresults.Add(onepValidationresult02);
        }

        [HttpPut("EditNetwork/{id}/EditVPs2")]
        public async Task<object> EditVPs2(long id, [FromBody] string value)
        {
            var session = _sessionFactory.OpenSession();
            var onepNetwork = session.Get<OnepNetwork>(id);

            // Replace TP01 by new TP03, but move the same AmpRole object to TP03
            EditNetworkEditVPs(onepNetwork);

            session.Close(); // Evict

            var session2 = _sessionFactory.OpenSession();
            using (var tx = session2.BeginTransaction())
            {
                try
                {

                    //// Replace TP01 by new TP03, but move the same AmpRole object to TP03
                    //EditNetworkEditVPs(onepNetwork);

                    await session2.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session2.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session2.Close();

                    throw;
                }
            }
            return new { Id = id };

        }


        [HttpPut("EditNetwork/{id}/EditVPs3")]
        public async Task<object> EditVPs3(long id, [FromBody] string value)
        {
            var session = _sessionFactory.OpenSession();
            var onepNetwork = session.Get<OnepNetwork>(id);

            // Replace TP01 by new TP03, but move the same AmpRole object to TP03
            EditNetworkEditVPs(onepNetwork);

            session.Close(); // Evict

            var session2 = _sessionFactory.OpenSession();
            using (var tx = session2.BeginTransaction())
            {
                try
                {

                    //// Replace TP01 by new TP03, but move the same AmpRole object to TP03
                    //EditNetworkEditVPs(onepNetwork);

                    await session2.SaveOrUpdateAsync(onepNetwork).ConfigureAwait(false);
                    tx.Commit();
                    session2.Close();

                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session2.Close();

                    throw;
                }
            }
            return new { Id = id };

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            var session = _sessionFactory.OpenSession();
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    var onepNetwork = session.Get<OnepNetwork>(id);
                    session.Delete(onepNetwork);
                    tx.Commit();
                    session.Close();
                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                    session.Close();

                    throw;
                }
            }
        }
    }
}
