
namespace OnepMini.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using NHibernate;
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

        // GET: api/Reports/GetFibersReport
        [HttpGet("GetFibersReport")]
        public async Task<IActionResult> GetFibersReport(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid planning project ID provided");
            }

            var root = _session.Query<ReportingRoot>().FirstOrDefault(p => p.ProjectId == projectId.ToString());

            //Debug.WriteLine($"{root.FibersReport.Data.Count}");

            if (root == null || root.FibersReport == null)
            {
                return BadRequest($"FibersReport not found for projectId: {projectId}");
            }

            await Task.Delay(0);

            return Ok(root.FibersReport);
        }

        // POST: api/Reports/GetFibersReport
        [HttpPost("GenerateFibersReport")]
        public async Task<IActionResult> GenerateFibersReport(Guid projectId)
        {
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
                MyListOfStrings2 = new List<string> { "DD", "EE"}
            };

            var reportingRoot = new ReportingRoot()
            {
                ProjectId = projectId.ToString(),
                CreationDate = DateTimeOffset.Now,
                LastAccessedDate = DateTimeOffset.Now,
                FibersReport = report
            };

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
                return BadRequest($"FibersReport not found for projectId: {projectId}");
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
    }
}
