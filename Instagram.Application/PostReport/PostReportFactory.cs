using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;

namespace Instagram.Application
{
    public class PostReportFactory : IPostReportFactory
    {
        public PostReport Create(AddPostReportRequest request)
            => new PostReport
            {
                PostId = request.PostId,
                Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Reason = request.Reason,
                ReportingUserId = request.ReportingUserId,
                Resolved = false
            };
    }
}
