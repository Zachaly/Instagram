using Instagram.Application;
using Instagram.Models.PostReport.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class PostReportFactoryTests
    {
        private readonly PostReportFactory _factory;

        public PostReportFactoryTests()
        {
            _factory = new PostReportFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddPostReportRequest
            {
                PostId = 1,
                Reason = "reason",
                ReportingUserId = 2
            };

            var report = _factory.Create(request);

            Assert.Equal(request.PostId, report.PostId);
            Assert.Equal(request.Reason, report.Reason);
            Assert.Equal(request.ReportingUserId, report.ReportingUserId);
            Assert.False(report.Resolved);
            Assert.Null(report.ResolveTime);
            Assert.Null(report.Accepted);
        }
    }
}
