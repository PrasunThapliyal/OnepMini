
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

            var fibersReport = _session.Query<FibersReport>().Where(p => p.McpProjectId == projectId.ToString()).FirstOrDefault();

            if (fibersReport == null)
            {
                return BadRequest($"FibersReport not found for projectId: {projectId}");
            }

            await Task.Delay(0);

            return Ok(fibersReport);
        }

        // POST: api/Reports/GetFibersReport
        [HttpPost("GenerateFibersReport")]
        public async Task<IActionResult> GenerateFibersReport(Guid projectId)
        {
            var report = new FibersReport()
            {
                McpProjectId = projectId.ToString(),
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
                }
            };

            using var tx = _session.BeginTransaction();
            try
            {
                await _session.SaveOrUpdateAsync(report).ConfigureAwait(false);

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

    }
}
