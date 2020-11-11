
namespace OnepMini.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    //using Newtonsoft.Json;
    //using Newtonsoft.Json;
    using NHibernate;
    using NHibernate.Criterion;
    using NHibernate.Linq;
    using NHibernate.Transform;
    using OnepMini.OrmNhib.DummyReports;
    using OnepMini.OrmNhib.Initializer;
    using OnepMini.V1.Common;
    using OnepMini.V1.Etp.Common;
    using OnepMini.V1.Etp.Reports;

    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        private readonly NHibernate.Cfg.Configuration _configuration;

        public ReportsController(
            INHibernateInitializer nHibernateInitializer,
            ISessionFactory sessionFactory)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            this._session = _sessionFactory.OpenSession();
            _configuration = _nHibernateInitializer.GetConfiguration();
        }

        [HttpGet("GetEquipmentReport_Working_ImproveEfficiency")]
        public async Task<IActionResult> GetEquipmentReport_Working_ImproveEfficiency(Guid projectId)
        {
            // TBD: You need 'set' instead of 'bag' in HBM to do this, and 'ICollection' instead of 'IList' in .cs

            try
            {
                var data = await _session.CreateSQLQuery(
                    "select * from equipment_report_item e0 inner join equipment_spec es1 on e0.equipmentSpec = es1.oid and es1.partNumber = 'NTK554BA' left outer join equipment_report er on e0.equipmentReport = 196608;")
                    //"select * from equipment_report er left outer join equipment_report_item e0 on e0.equipmentReport = er.oid and er.oid = 196608 inner join equipment_spec es1 on e0.equipmentSpec = es1.oid and es1.partNumber = 'NTK554BA';")
                    .AddEntity("e0", typeof(EquipmentReportItem))
                    .ListAsync<EquipmentReportItem>();

                var reports = await _session.Query<EquipmentReport>()
                    .Where(p => p.OId == 196608)
                    .ToListAsync<EquipmentReport>();

                var report = reports.FirstOrDefault();
                report.Data = data;
                report.ReportingMetaInfo = new V1.Etp.Common.ReportingMetaInfo
                {
                    TotalRecordsInReport = report.Data.Count
                };

                return Ok(report);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }

            //var subQuery = await _session.Query<EquipmentReportItem>()
            //    .Where(r => r.EquipmentSpec.PartNumber == "NTK554BA").ToListAsync();

            //var reports = _session.Query<EquipmentReport>()
            //    .FetchMany(p => subQuery)
            //    .Where(p => p.Parameters.Count >= 0)
            //    .Skip(0).Take(50)
            //    .ToFuture<EquipmentReport>();

            //_session.Query<EquipmentSpec>()
            //    .Where(p => p.PartNumber == "NTK554BA")
            //    .ToFutureValue();

            //_session.Query<EquipmentReportItem>().Fetch(p => p.EquipmentSpec).ToFuture();
            //_session.Query<EquipmentReportItem>().Fetch(p => p.EligibleNEs).ToFuture();
            //_session.Query<EquipmentReportItem>().FetchMany(p => p.EligibleNEs).ThenFetch(q => q.EligibleShelves).ToFuture();
            //_session.Query<EquipmentReportItem>().Fetch(p => p.EligibleShelves).ToFuture();
            //_session.Query<EquipmentReportItem>().Fetch(p => p.Location).ThenFetch(q => q.Address).ToFuture();
            //_session.Query<EquipmentReportItem>().Fetch(p => p.Node).ToFuture();
            //_session.Query<EquipmentReportItem>().Fetch(p => p.PlanningState).ThenFetch(q => q.PlanningPhase).ToFuture();

            //var report = reports.FirstOrDefault();
            //report.ReportingMetaInfo = new V1.Etp.Common.ReportingMetaInfo
            //{
            //    TotalRecordsInReport = report.Data.Count
            //};

            await Task.Delay(0);

            return Ok();
        }

        [HttpGet("GetEquipmentReport_UsingFutures")]
        public async Task<IActionResult> GetEquipmentReport_UsingFutures(Guid projectId)
        {
            // Works quite well ..

            // TBD: You need 'set' instead of 'bag' in HBM to do this, and 'ICollection' instead of 'IList' in .cs

            var reports = _session.Query<EquipmentReport>()
                .Where(p => p.Parameters.Count >= 0)
                .FetchMany(p => p.Data)
                .Skip(0).Take(50)
                .ToFuture<EquipmentReport>();

            _session.Query<EquipmentReportItem>().Fetch(p => p.EquipmentSpec).ToFuture();
            _session.Query<EquipmentReportItem>().Fetch(p => p.EligibleNEs).ToFuture();
            _session.Query<EquipmentReportItem>().FetchMany(p => p.EligibleNEs).ThenFetch(q => q.EligibleShelves).ToFuture();
            _session.Query<EquipmentReportItem>().Fetch(p => p.EligibleShelves).ToFuture();
            _session.Query<EquipmentReportItem>().Fetch(p => p.Location).ThenFetch(q => q.Address).ToFuture();
            _session.Query<EquipmentReportItem>().Fetch(p => p.Node).ToFuture();
            _session.Query<EquipmentReportItem>().Fetch(p => p.PlanningState).ThenFetch(q => q.PlanningPhase).ToFuture();

            var report = reports.FirstOrDefault();
            report.ReportingMetaInfo = new V1.Etp.Common.ReportingMetaInfo
            {
                TotalRecordsInReport = report.Data.Count
            };

            await Task.Delay(0);

            return Ok(report);
        }

        [HttpGet("GetEquipmentReport_SQL_FullJoins")]
        public async Task<IActionResult> GetEquipmentReport_SQL_FullJoins(Guid projectId)
        {
            // This is giving me 175 rows of data, as expected
            // But there are some more queries when un-proxying / serializing

            var reportingRoot = await _session.Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            EquipmentReport report =
                reportingRoot.EquipmentReports.FirstOrDefault();

            var sqlQuery =
"    select * " +
"    from " +
"        equipment_report equipmentr0_ " +
"    inner join " +
"        equipment_report_item data1_ " +
"            on (equipmentr0_.oid = data1_.equipmentReport and data1_.equipmentgroupkey = 'Site7_NE7_1_1') " +
"    inner join " +
"        equipment_spec equipments2_ " +
"            on data1_.equipmentSpec = equipments2_.oid " +
"    inner join " +
"        eligible_ne eligiblene3_ " +
"            on data1_.oid = eligiblene3_.equipmentReportItem " +
"    inner join " +
"        eligible_ne_eligible_shelves eligiblesh4_ " +
"            on eligiblene3_.oid = eligiblesh4_.eligibleNE " +
"    inner join " +
"        location location5_ " +
"            on data1_.location = location5_.oid " +
"    inner join " +
"        location_address address6_ " +
"            on location5_.oid = address6_.location " +
"    inner join " +
"        node_identity nodeidenti7_ " +
"            on data1_.node = nodeidenti7_.oid " +
"    inner join " +
"        planning_state planningst8_ " +
"            on data1_.planningState = planningst8_.oid " +
"    inner join " +
"        planning_phase planningph9_ " +
"            on planningst8_.planningPhase = planningph9_.oid " +
"    inner join " +
"        equipment_report_item_eligible_shelves eligiblesh10_ " +
"            on data1_.oid = eligiblesh10_.equipmentReportItem " +
"   ; "
            ;

            var equipmentr0_ = await _session.CreateSQLQuery(sqlQuery)
                .AddEntity("equipmentr0_", typeof(EquipmentReport))
                //.AddEntity("data1_", typeof(EquipmentReportItem))
                //.AddEntity("equipments2_", typeof(EquipmentSpec))
                //.AddEntity("eligiblene3_", typeof(EligibleNE))
                ////.AddEntity("eligiblesh4_", typeof(Eli))
                //.AddEntity("location5_", typeof(Location))
                //.AddEntity("address6_", typeof(LocationAddress))
                //.AddEntity("nodeidenti7_", typeof(NodeIdentity))
                //.AddEntity("planningst8_", typeof(PlanningState))
                //.AddEntity("planningph9_", typeof(PlanningPhase))
                ////.AddEntity("eligiblesh10_", typeof(LocationAddress))
                .ListAsync<EquipmentReport>().ConfigureAwait(false);

            equipmentr0_[0].ReportingMetaInfo = new V1.Etp.Common.ReportingMetaInfo
            {
                TotalRecordsInReport = equipmentr0_[0].Data.Count
            };

            var sw = Stopwatch.StartNew();

            var x = System.Text.Json.JsonSerializer.Serialize<EquipmentReport>(equipmentr0_[0]);
            var y = System.Text.Json.JsonSerializer.Deserialize<EquipmentReport>(x);

            Console.WriteLine($"UnProxy took {sw.ElapsedMilliseconds} ms");

            return Ok(y);
        }

        [HttpGet("GetEquipmentReport_SQLWorking_FullJoins")]
        public async Task<IActionResult> GetEquipmentReport_SQLWorking_FullJoins(Guid projectId)
        {
            // This is giving me 175 rows of data, as expected

            var reportingRoot = await _session.Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            EquipmentReport report =
                reportingRoot.EquipmentReports.FirstOrDefault();

            var sqlQuery =
"    select * " +
"    from " +
"        equipment_report equipmentr0_ " +
"    left outer join " +
"        equipment_report_item data1_ " +
"            on equipmentr0_.oid = data1_.equipmentReport " +
"    left outer join " +
"        equipment_spec equipments2_ " +
"            on data1_.equipmentSpec = equipments2_.oid " +
"    left outer join " +
"        eligible_ne eligiblene3_ " +
"            on data1_.oid = eligiblene3_.equipmentReportItem " +
"    left outer join " +
"        eligible_ne_eligible_shelves eligiblesh4_ " +
"            on eligiblene3_.oid = eligiblesh4_.eligibleNE " +
"    left outer join " +
"        location location5_ " +
"            on data1_.location = location5_.oid " +
"    left outer join " +
"        location_address address6_ " +
"            on location5_.oid = address6_.location " +
"    left outer join " +
"        node_identity nodeidenti7_ " +
"            on data1_.node = nodeidenti7_.oid " +
"    left outer join " +
"        planning_state planningst8_ " +
"            on data1_.planningState = planningst8_.oid " +
"    left outer join " +
"        planning_phase planningph9_ " +
"            on planningst8_.planningPhase = planningph9_.oid " +
"    left outer join " +
"        equipment_report_item_eligible_shelves eligiblesh10_ " +
"            on data1_.oid = eligiblesh10_.equipmentReportItem " +
"   ; "
            ;

            var equipmentr0_ = await _session.CreateSQLQuery(sqlQuery)
                .AddEntity("equipmentr0_", typeof(EquipmentReport))
                .ListAsync<EquipmentReport>().ConfigureAwait(false);

            var sw = Stopwatch.StartNew();

            var x = System.Text.Json.JsonSerializer.Serialize<EquipmentReport>(equipmentr0_[0]);
            var y = System.Text.Json.JsonSerializer.Deserialize<EquipmentReport>(x);

            Console.WriteLine($"UnProxy took {sw.ElapsedMilliseconds} ms");

            return Ok(y);
        }

        [HttpGet("GetEquipmentReport_LINQ")]
        public async Task<IActionResult> GetEquipmentReport_LINQ(Guid projectId)
        {
            var reportingRoot = await _session.Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            // You need 'set' instead of 'bag' in HBM to do this, and 'ICollection' instead of 'IList' in .cs

            var report = await _session.Query<EquipmentReport>()
                .FetchMany(p => p.Data)
                .ThenFetch(q => q.EquipmentSpec)

                .FetchMany(p => p.Data)
                .ThenFetchMany(q => q.EligibleNEs)
                .ThenFetch(r => r.EligibleShelves)

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.Location)
                .ThenFetch(r => r.Address)

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.Node)


                .FetchMany(p => p.Data)
                .ThenFetch(q => q.PlanningState)
                .ThenFetch(r => r.PlanningPhase)


                .FetchMany(p => p.Data)
                .ThenFetch(q => q.EligibleShelves)

                //.Skip(0).Take(50)
                .ToListAsync();

            return Ok(report.FirstOrDefault());
        }

        [HttpGet("GetEquipmentReport_WrongSQL")]
        public async Task<IActionResult> GetEquipmentReport_WrongSQL(Guid projectId)
        {
            // I copied the SQL from console of the _LINQ_SUBQUERY example
            // but it produces 68K rows whereas there are only 175 EquipmentReportItem rows
            // Perhaps, its the cartesian product due to too many joins
            // In _LINQ_SUBQUERY, I guess NH handles duplicates because of 'set' usage
            // so it returns the correct 175 count

            var reportingRoot = await _session.Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            EquipmentReport report =
                reportingRoot.EquipmentReports.FirstOrDefault();

            var sqlQuery =
"    select * " +
"    from " +
"        equipment_report equipmentr0_ " +
"    left outer join " +
"        equipment_report_item data1_ " +
"            on equipmentr0_.oid = data1_.equipmentReport " +
"    left outer join " +
"        equipment_report_item data2_ " +
"            on equipmentr0_.oid = data2_.equipmentReport " +
"    left outer join " +
"        equipment_spec equipments3_ " +
"            on data2_.equipmentSpec = equipments3_.oid " +
"    left outer join " +
"        eligible_ne eligiblene4_ " +
"            on data2_.oid = eligiblene4_.equipmentReportItem " +
"    left outer join " +
"        eligible_ne_eligible_shelves eligiblesh5_ " +
"            on eligiblene4_.oid = eligiblesh5_.eligibleNE " +
"    left outer join " +
"        location location6_ " +
"            on data2_.location = location6_.oid " +
"    left outer join " +
"        location_address address7_ " +
"            on location6_.oid = address7_.location " +
"    left outer join " +
"        node_identity nodeidenti8_ " +
"            on data2_.node = nodeidenti8_.oid " +
"    left outer join " +
"        planning_state planningst9_ " +
"            on data2_.planningState = planningst9_.oid " +
"    left outer join " +
"        planning_phase planningph10_ " +
"            on planningst9_.planningPhase = planningph10_.oid " +
"    left outer join " +
"        equipment_report_item_eligible_shelves eligiblesh11_ " +
"            on data2_.oid = eligiblesh11_.equipmentReportItem " +
"    where " +
"        equipmentr0_.oid in ( 196608 )" +
"                ; "
            ;

            var data = await _session.CreateSQLQuery(sqlQuery)
                .AddEntity("data1_", typeof(EquipmentReportItem))
                .ListAsync<EquipmentReportItem>().ConfigureAwait(false);

            var countSqlQuery =
                $"select count(*) from equipment_report_item data " +
                $"where equipmentreport = {report.OId} " +
                ";";

            var countFilteredData = (long)(_session.CreateSQLQuery(countSqlQuery).UniqueResult());

            report.Data = data;
            report.ReportingMetaInfo = new OnepMini.V1.Etp.Common.ReportingMetaInfo
            {
                TotalRecordsInDB = countFilteredData,
                TotalRecordsInReport = data?.Count,
                PageNumber = 1,
                PageSize = 50
            };

            var sw = Stopwatch.StartNew();

            var x = System.Text.Json.JsonSerializer.Serialize<EquipmentReport>(report);
            var y = System.Text.Json.JsonSerializer.Deserialize<EquipmentReport>(x);

            Console.WriteLine($"UnProxy took {sw.ElapsedMilliseconds} ms");

            return Ok(y);
        }

        [HttpGet("GetEquipmentReport_LINQ_SUBQUERY")]
        public async Task<IActionResult> GetEquipmentReport_LINQ_SUBQUERY(Guid projectId)
        {
            // You need 'set' instead of 'bag' in HBM to do this, and 'ICollection' instead of 'IList' in .cs

            var subQuery = await _session.Query<EquipmentReport>()
                .Where(p => p.Parameters.Count >= 0)
                .FetchMany(p => p.Data)
                //.Skip(0).Take(50)
                .ToListAsync();


            var report = await _session.Query<EquipmentReport>()
                .FetchMany(p => p.Data)
                .Where(x => subQuery.Contains(x))

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.EquipmentSpec)

                .FetchMany(p => p.Data)
                //.ThenFetchMany(q => q.EligibleNEs.Where(ne => ne.Tid == "NE4")) // Where clause wont work in NH
                .ThenFetchMany(q => q.EligibleNEs)
                .ThenFetch(r => r.EligibleShelves)

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.Location)
                .ThenFetch(r => r.Address)

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.Node)


                .FetchMany(p => p.Data)
                .ThenFetch(q => q.PlanningState)
                .ThenFetch(r => r.PlanningPhase)


                .FetchMany(p => p.Data)
                .ThenFetch(q => q.EligibleShelves)

                .ToListAsync();

            report[0].ReportingMetaInfo = new V1.Etp.Common.ReportingMetaInfo
            {
                TotalRecordsInReport = report[0].Data.Count
            };

            return Ok(report.FirstOrDefault());
        }

        [HttpGet("GetEquipmentReport_EagerLoadAllChildCollections")]
        public async Task<IActionResult> GetEquipmentReport_EagerLoadAllChildCollections(Guid projectId)
        {
            var reportingRoot = await _session.Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            // You need 'set' instead of 'bag' in HBM to do this, and 'ICollection' instead of 'IList' in .cs

            var report = await _session.Query<EquipmentReport>()
                .FetchMany(p => p.Data)
                .ThenFetch(q => q.EquipmentSpec)
                
                .FetchMany(p => p.Data)
                .ThenFetchMany(q => q.EligibleNEs)
                .ThenFetch(r => r.EligibleShelves)

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.Location)
                .ThenFetch(r => r.Address)

                .FetchMany(p => p.Data)
                .ThenFetch(q => q.Node)


                .FetchMany(p => p.Data)
                .ThenFetch(q => q.PlanningState)
                .ThenFetch(r => r.PlanningPhase)


                .FetchMany(p => p.Data)
                .ThenFetch(q => q.EligibleShelves)

                .Skip(0).Take(50)
                .ToListAsync();

            return Ok(report.FirstOrDefault());
        }

        [HttpGet("GetEquipmentReport_SQL_Working")]
        public async Task<IActionResult> GetEquipmentReport_SQL_Working(Guid projectId)
        {
            var reportingRoot = await _session.Query<ReportingRoot>() 
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            EquipmentReport report =
                reportingRoot.EquipmentReports.FirstOrDefault();

            var sqlQuery = $"select * from equipment_report_item data where equipmentreport = {report.OId} limit 50 offset 0";

            var data = await _session.CreateSQLQuery(sqlQuery)
                .AddEntity("data", typeof(EquipmentReportItem))
                .ListAsync<EquipmentReportItem>().ConfigureAwait(false);

            sqlQuery =
                $"select count(*) from equipment_report_item data " +
                $"where equipmentreport = {report.OId} " +
                ";";

            var countFilteredData = (long)(_session.CreateSQLQuery(sqlQuery).UniqueResult());

            report.Data = data;
            report.ReportingMetaInfo = new OnepMini.V1.Etp.Common.ReportingMetaInfo
            {
                TotalRecordsInDB = countFilteredData,
                TotalRecordsInReport = data?.Count,
                PageNumber = 1,
                PageSize = 50
            };

            var sw = Stopwatch.StartNew();

            var x = System.Text.Json.JsonSerializer.Serialize<EquipmentReport>(report);
            var y = System.Text.Json.JsonSerializer.Deserialize<EquipmentReport>(x);

            Console.WriteLine($"UnProxy took {sw.ElapsedMilliseconds} ms");

            return Ok(report);
        }

#if DONT_COMPILE

        // POST: api/Reports/GenerateCSAmpProvisioningReport
        [HttpPost("GenerateCSAmpProvisioningReport")]
        public async Task<IActionResult> GenerateCSAmpProvisioningReport(Guid projectId)
        {
            CSAmpProvisioningReport report = null;
            {
                var assembly = GetType().GetTypeInfo().Assembly;
                var assemblyName = assembly.GetName().Name;
                var resourceName = $"{assemblyName}.OrmNhib.DummyReports.dummyCSAmpProvReport.json";

                report = JsonReader.ReadJsonDataFile<CSAmpProvisioningReport>(assembly, resourceName);
            }

            ReportingRoot reportingRoot = new ReportingRoot()
            {
                ProjectId = projectId.ToString(),
                CreationDate = DateTimeOffset.Now,
                LastAccessedDate = DateTimeOffset.Now
            };
            reportingRoot.CsAmpProvisioningReports.Add(report);

            using var tx = _session.BeginTransaction();
            try
            {
                await _session.SaveOrUpdateAsync(reportingRoot).ConfigureAwait(false);

                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debugger.Break();
                Debug.WriteLine(ex);

                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }

            return Ok();
        }


        // GET: api/Reports/GetCSAmpProvisioningReport
        [HttpGet("GetCSAmpProvisioningReport")]
        public async Task<IActionResult> GetCSAmpProvisioningReport(Guid projectId, bool includePowerTable = false, string type = "json", string reportingParams = null)
        {
            var reportingParamsOject = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportingParams>(reportingParams);

            var reportingRoot = await _session.Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);
            /*
                NHibernate:
                    select
                        reportingr0_.oid as oid1_10_,
                        reportingr0_.projectId as projectid2_10_,
                        reportingr0_.creationDate as creationdate3_10_,
                        reportingr0_.lastAccessedDate as lastaccesseddate4_10_
                    from
                        reporting_root reportingr0_
                    where
                        reportingr0_.projectId=:p0 limit 1;
                    :p0 = 'a71353de-47e8-47dd-9e09-a45a19e220c3' [Type: String (0:0:0)]
             * 
             * */


            if (reportingRoot == null)
                return null;

            var requiredParams = new List<UrlParameter>
            {
                new UrlParameter { Name= "includePowerTable", Value = $"{includePowerTable}" },
                new UrlParameter { Name= "type", Value = $"{type}" }
            };

            CSAmpProvisioningReport cSAmpProvisioningReport =
                reportingRoot.CsAmpProvisioningReports.FirstOrDefault(
                    report => report.Parameters.All(
                        reportParam => requiredParams.Any(
                            requireParam => requireParam.Name == reportParam.Name && requireParam.Value == reportParam.Value)
                        ));
            /*
                NHibernate:
                    SELECT
                        csampprovi0_.reportingRoot as reportingroot3_1_1_,
                        csampprovi0_.oid as oid1_1_1_,
                        csampprovi0_.oid as oid1_1_0_,
                        csampprovi0_.reportingMetaInfo as reportingmetainfo2_1_0_
                    FROM
                        csamp_provisioning_report csampprovi0_
                    WHERE
                        csampprovi0_.reportingRoot=:p0;
                    :p0 = 32768 [Type: Int64 (0:0:0)]
                NHibernate:
                    SELECT
                        parameters0_.cSAmpProvisioningReport as csampprovisioningreport4_11_1_,
                        parameters0_.oid as oid1_11_1_,
                        parameters0_.oid as oid1_11_0_,
                        parameters0_.name as name2_11_0_,
                        parameters0_.value as value3_11_0_
                    FROM
                        url_parameter parameters0_
                    WHERE
                        parameters0_.cSAmpProvisioningReport=:p0;
                    :p0 = 65536 [Type: Int64 (0:0:0)]
             * 
             * */

            var csAmpProvOID = cSAmpProvisioningReport.OId;

            // Filtering, Sorting, Pagination should be applied in this order

            var filterColumn = "sitename";
            var filterType = "like";
            var filterText = "Site";

            var orderByColumn = "ampdirection";
            var orderDirection = "asc";

            var pageSize = reportingParamsOject.pageSize;
            var pageNumber = reportingParamsOject.pageNumber;

            pageSize = 2;
            pageNumber = 1;

            var sqlQuery =
                $"select * from csamp_provisioning_report_item data " +
                $"where csampprovisioningreport = {csAmpProvOID} " +
                $"and data.{filterColumn} {filterType} \'%{filterText}%\' " + // Filter
                $"order by {orderByColumn} {orderDirection} " + // Sort
                $"limit {pageSize} offset {pageSize * (pageNumber - 1)};"; // Paging

            var data = await _session.CreateSQLQuery(sqlQuery)
                .AddEntity("data", typeof(CSAmpProvisioningReportItem))
                .SetResultTransformer(Transformers.DistinctRootEntity)
                .ListAsync<CSAmpProvisioningReportItem>();


            var x = await _session.CreateSQLQuery(
                "select sitename, count(*) from csamp_provisioning_report_item data where csampprovisioningreport = 65536 group by sitename; ").ListAsync<object>();

            var xString = Newtonsoft.Json.JsonConvert.SerializeObject(x);

            /*
                NHibernate:
                    select
                        *
                    from
                        csamp_provisioning_report_item data
                    where
                        csampprovisioningreport = 65536
                        and data.sitename like '%Site%' limit 1 offset 0;
             * 
             * */

            cSAmpProvisioningReport.Data = data;

            return Ok(cSAmpProvisioningReport);
        }


        // GET: api/Reports/GetFibersReport
        [HttpGet("GetFibersReport")]
        public async Task<IActionResult> GetFibersReport(Guid projectId, int pageNumber = 0, int pageSize = 0)
        {
            Console.WriteLine($"GetFibersReport: {projectId}, pageNumber {pageNumber}, pageSize {pageSize}");

            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid planning project ID provided");
            }

            var root = await _session
                .Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            //Debug.WriteLine($"{root.FibersReport.Data.Count}");

            if (root == null || root.FibersReport == null)
            {
                return BadRequest($"FibersReport not found for projectId: {projectId}");
            }

            if (pageNumber > 0 && pageSize > 0)
            {
                // We want to return root.FibersReport
                // Where pagination is applied to .Data
                // Something like this, except that here .Skip and .Take are being done in user code

                //root.FibersReport.Data = root.FibersReport.Data.Skip(pageNumber * pageSize).Take(pageSize).ToList();

                // We want the SQL to be such that only 1 page data is fetched from root.FibersReport.Data
                // Something like this

                /*
                 * 
                    select * from fibers_report_item 
	                    where fibersreport =
		                    (select oid from fibers_report where oid = 
			                    (select fibersreport from reporting_root where projectId='7483d4d8-6e7a-4df1-8eeb-d829ff0452bb')
		                    )
                    limit 1 offset 2;
                 * 
                 * */

                // Which, btw, can be further pruned by safely removing the middle subquery

                var sqlQuery =
                    $"select * from fibers_report_item data " + 
                    $"where fibersreport=" + 
                    $"(select fibersreport from reporting_root where projectId=\'{projectId}\') " + 
                    $"limit {pageSize} offset {pageSize*pageNumber};";

                var data = await _session.CreateSQLQuery(sqlQuery)
                    .AddEntity("data", typeof(FibersReportItem))
                    .SetResultTransformer(Transformers.DistinctRootEntity)
                    .ListAsync<FibersReportItem>();

                root.FibersReport.Data = data;
            }

            return Ok(root.FibersReport);
        }

        // POST: api/Reports/GetFibersReport
        [HttpPost("GenerateFibersReport")]
        public async Task<IActionResult> GenerateFibersReport(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid planning project ID provided");
            }

            var report = new FibersReport()
            {
                Data = new List<FibersReportItem>()
                {
                    new FibersReportItem()
                    {
                        Name = "FS157_3",
                        FiberId = "33564",
                        FiberPairString = "FS157_1;FS158_1",
                        FiberType = "TERA",
                        OnepSiteByAEndSite = "Tornillo",
                        OnepSiteByZEndSite = "West El Paso",
                        FiberLength =  (decimal)70.60000,
                        TotalLoss =  (decimal)17.17900,
                        FiberLossSourceType = "FromLength",
                        FiberLoss = (decimal)  15.17900 ,
                        LossPerKm = (decimal)  0.21500,
                        MarginDbPerSpan = (decimal)       1.00000,
                        MarginDbPerKm = (decimal) 0.01416,
                        MarginSourceType = "Manual",
                        HeadPPL = (decimal)0.50000,
                        TailPPL = (decimal) 0.50000 ,
                        PmdType = "FromLength",
                        PmdCoefficient = (decimal) 0.10000,
                        PmdMean = (decimal) 0.84024

                    },
                    new FibersReportItem()
                    {
                        Name = "FS157_4",
                        FiberId = "33565",
                        FiberPairString = "FS157_1;FS158_1",
                        FiberType = "TERA",
                        OnepSiteByAEndSite = "West El Paso",
                        OnepSiteByZEndSite = "Tornillo",
                        FiberLength =  (decimal)70.60000,
                        TotalLoss =  (decimal)17.17900,
                        FiberLossSourceType = "FromLength",
                        FiberLoss = (decimal)  15.17900 ,
                        LossPerKm = (decimal)  0.21500,
                        MarginDbPerSpan = (decimal)       1.00000,
                        MarginDbPerKm = (decimal) 0.01416,
                        MarginSourceType = "Manual",
                        HeadPPL = (decimal)0.50000,
                        TailPPL = (decimal) 0.50000 ,
                        PmdType = "FromLength",
                        PmdCoefficient = (decimal) 0.10000,
                        PmdMean = (decimal) 0.84024

                    },
                    new FibersReportItem()
                    {
                        Name = "FS157_5",
                        FiberId = "33564",
                        FiberPairString = "FS157_1;FS158_1",
                        FiberType = "TERA",
                        OnepSiteByAEndSite = "Tornillo",
                        OnepSiteByZEndSite = "West El Paso",
                        FiberLength =  (decimal)70.60000,
                        TotalLoss =  (decimal)17.17900,
                        FiberLossSourceType = "FromLength",
                        FiberLoss = (decimal)  15.17900 ,
                        LossPerKm = (decimal)  0.21500,
                        MarginDbPerSpan = (decimal)       1.00000,
                        MarginDbPerKm = (decimal) 0.01416,
                        MarginSourceType = "Manual",
                        HeadPPL = (decimal)0.50000,
                        TailPPL = (decimal) 0.50000 ,
                        PmdType = "FromLength",
                        PmdCoefficient = (decimal) 0.10000,
                        PmdMean = (decimal) 0.84024

                    },
                    new FibersReportItem()
                    {
                        Name = "FS157_6",
                        FiberId = "33565",
                        FiberPairString = "FS157_1;FS158_1",
                        FiberType = "TERA",
                        OnepSiteByAEndSite = "West El Paso",
                        OnepSiteByZEndSite = "Tornillo",
                        FiberLength =  (decimal)70.60000,
                        TotalLoss =  (decimal)17.17900,
                        FiberLossSourceType = "FromLength",
                        FiberLoss = (decimal)  15.17900 ,
                        LossPerKm = (decimal)  0.21500,
                        MarginDbPerSpan = (decimal)       1.00000,
                        MarginDbPerKm = (decimal) 0.01416,
                        MarginSourceType = "Manual",
                        HeadPPL = (decimal)0.50000,
                        TailPPL = (decimal) 0.50000 ,
                        PmdType = "FromLength",
                        PmdCoefficient = (decimal) 0.10000,
                        PmdMean = (decimal) 0.84024

                    }
                },
                MyListOfStrings1 = new List<string> { "AA", "BB", "CC"},
                MyListOfStrings2 = new List<string> { "DD", "EE"},
                AcctType = FibersReport.AccountTypes.Corporate
            };

            var reportingRoot = _session.Query<ReportingRoot>()
                .FirstOrDefault(p => p.ProjectId == projectId.ToString());

            if (reportingRoot == null)
            {
                reportingRoot = new ReportingRoot()
                {
                    ProjectId = projectId.ToString(),
                    CreationDate = DateTimeOffset.Now,
                    LastAccessedDate = DateTimeOffset.Now,
                    FibersReport = report
                };
            }
            else
            {
                if (reportingRoot.FibersReport != null)
                {
                    using var tx1 = _session.BeginTransaction();
                    try
                    {
                        await _session.DeleteAsync(reportingRoot.FibersReport).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Debugger.Break();
                        Debug.WriteLine(ex);

                        await tx1.RollbackAsync().ConfigureAwait(false);
                        throw;
                    }
                }
                //reportingRoot.LastAccessedDate = DateTimeOffset.Now;
                reportingRoot.FibersReport = report;
            }

            using var tx = _session.BeginTransaction();
            try
            {
                await _session.SaveOrUpdateAsync(reportingRoot).ConfigureAwait(false);

                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debugger.Break();
                Debug.WriteLine(ex);

                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }

            return Ok();
        }


        // GET: api/Reports/GetOchReport
        [HttpGet("GetOchReport")]
        public async Task<IActionResult> GetOchReport(Guid projectId, int pageNumber = 0, int pageSize = 0)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid planning project ID provided");
            }

            var root = await _session
                .Query<ReportingRoot>()
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.ToString()).ConfigureAwait(false);

            if (root == null || root.OchReport == null)
            {
                return BadRequest($"OchReport not found for projectId: {projectId}");
            }

            //if (pageNumber > 0 && pageSize > 0)
            //{
            //    return Ok(root.OchReport.Data.Skip(pageNumber * pageSize).Take(pageSize));
            //}

            return Ok(root.OchReport);
        }

        // POST: api/Reports/GenerateOchReport
        [HttpPost("GenerateOchReport")]
        public async Task<IActionResult> GenerateOchReport(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid planning project ID provided");
            }

            OchReport report = null;
            {
                var assembly = GetType().GetTypeInfo().Assembly;
                var assemblyName = assembly.GetName().Name;
                var resourceName = $"{assemblyName}.OrmNhib.DummyReports.dummyOchReport.json";

                report = JsonReader.ReadJsonDataFile<OchReport>(assembly, resourceName);
            }
            var reportingRoot = _session.Query<ReportingRoot>()
                .FirstOrDefault(p => p.ProjectId == projectId.ToString());

            using var tx = _session.BeginTransaction();
            try
            {
                if (reportingRoot == null)
                {
                    reportingRoot = new ReportingRoot()
                    {
                        ProjectId = projectId.ToString(),
                        CreationDate = DateTimeOffset.Now,
                        LastAccessedDate = DateTimeOffset.Now,
                        OchReport = report
                    };
                }
                else
                {
                    if (reportingRoot.OchReport != null)
                    {
                        await _session.DeleteAsync(reportingRoot.OchReport).ConfigureAwait(false);
                    }
                    reportingRoot.LastAccessedDate = DateTimeOffset.Now;
                    reportingRoot.OchReport = report;
                }

                await _session.SaveOrUpdateAsync(reportingRoot).ConfigureAwait(false);

                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debugger.Break();
                Debug.WriteLine(ex);

                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }

            return Ok();
        }


        [HttpDelete("DeleteAllReports")]
        public async Task<IActionResult> DeleteReportingRoot(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid planning project ID provided");
            }

            var root = _session.Query<ReportingRoot>().Where(p => p.ProjectId == projectId.ToString()).FirstOrDefault();

            if (root == null)
            {
                return BadRequest($"No reports found for projectId: {projectId}");
            }


            using var tx = _session.BeginTransaction();
            try
            {
                await _session.DeleteAsync(root).ConfigureAwait(false);

                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debugger.Break();
                Debug.WriteLine(ex);

                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }

            return NoContent();
        }

#endif

    }


    public class ReportingParams
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public List<Filtermodel> filterModel { get; set; }
        public List<Sortmodel> sortModel { get; set; }
    }

    public class Filtermodel
    {
        public string Column { get; set; }
        public List<string> Values { get; set; }
        public string FilterType { get; set; }
    }

    public class Sortmodel
    {
        public string Column { get; set; }
        public string Sort { get; set; }
    }

}
