using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.AccountVerification;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IAccountVerificationServiceProxy : IAccountVerificationService { }

    public class AccountVerificationServiceProxy :
        HttpLoggingServiceProxyBase<AccountVerificationModel, GetAccountVerificationRequest, IAccountVerificationService>,
        IAccountVerificationServiceProxy
    {
        public AccountVerificationServiceProxy(ILogger<IAccountVerificationService> logger, IHttpContextAccessor httpContextAccessor,
            IAccountVerificationService service) : base(logger, httpContextAccessor, service)
        {
            ServiceName = "AccountVerification";
        }
    }
}
