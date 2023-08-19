using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IRelationServiceProxy : IRelationService
    {

    }

    public class RelationServiceProxy : HttpLoggingServiceProxyBase<RelationModel, GetRelationRequest, IRelationService>, IRelationServiceProxy
    {
        public RelationServiceProxy(ILogger<IRelationServiceProxy> logger, IHttpContextAccessor httpContextAccessor, IRelationService relationService) 
            : base(logger, httpContextAccessor, relationService)
        {
            ServiceName = "Relation";
        }
    }
}
