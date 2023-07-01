using Instagram.Api.Authorization;
using Instagram.Application.Abstraction;
using Instagram.Models.UserBan.Request;
using System.Transactions;

namespace Instagram.Api.Infrastructure
{
    public class BanCancellationService : BackgroundService
    {
        const int Delay = 1000 * 60 * 60 * 24; // 24h
        private readonly IServiceProvider _serviceProvider;

        public BanCancellationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                using(var serviceScope = _serviceProvider.CreateScope())
                {
                    var logger = serviceScope.ServiceProvider.GetService<ILogger<BanCancellationService>>()!;
                    var claimService = serviceScope.ServiceProvider.GetService<IUserClaimService>()!;
                    var banService = serviceScope.ServiceProvider.GetService<IUserBanService>()!;
                    while(!stoppingToken.IsCancellationRequested)
                    {
                        var bans = await banService.GetAsync(new GetUserBanRequest { MinEndDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), SkipPagination = true });

                        foreach(var ban in bans)
                        {
                            using(var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                            {
                                await claimService.DeleteAsync(ban.UserId, UserClaimValues.Ban);
                                await banService.DeleteAsync(ban.Id);
                            }
                        }

                        logger.LogInformation("Unbanned {count} users", bans.Count());

                        await Task.Delay(Delay);
                    }
                }
            });
        }
    }
}
