using ReportData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BDZService.Controllers
{
    public class ReportController : ApiController
    {
        // POST api/report
        public void Post([FromBody]ReportDTO report)
        {
            Report newReport = new Report() { Title = report.title, ReportContnent = report.reportContent };
            ReportEntities reportEntitie = new ReportEntities();
            reportEntitie.Reports.Add(newReport);
            reportEntitie.SaveChanges();
            
        }
    }
}
