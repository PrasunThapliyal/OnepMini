
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
    using NHibernate;
    using NHibernate.Criterion;
    using NHibernate.Linq;
    using NHibernate.Transform;
    using OnepMini.OrmNhib.DummyReports;
    using OnepMini.OrmNhib.Initializer;
    using TopologyRestLibrary.V1.Etp.Reports;

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
            reportingRoot.CsAmpProvisioningReport.Add(report);

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
                reportingRoot.CsAmpProvisioningReport.FirstOrDefault(
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


#if DONT_COMPILE

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
