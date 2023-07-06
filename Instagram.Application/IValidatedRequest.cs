using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application
{
    public interface IValidatedRequest : IRequest<ResponseModel>
    {
    }
}
