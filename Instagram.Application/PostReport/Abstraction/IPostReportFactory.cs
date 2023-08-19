using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostReportFactory : IEntityFactory<PostReport, AddPostReportRequest>
    {

    }
}
