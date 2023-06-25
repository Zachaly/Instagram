using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostReportFactory
    {
        PostReport Create(AddPostReportRequest request);
    }
}
