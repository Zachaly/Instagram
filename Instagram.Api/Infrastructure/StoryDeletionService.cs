using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Models.UserStory.Request;
using MediatR;

namespace Instagram.Api.Infrastructure
{
    public class StoryDeletionService :  BackgroundService
    {
        const int Delay = 1000 * 60 * 60 * 24; // 24h
        private readonly IServiceProvider _serviceProvider;

        public StoryDeletionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var storyRepository = scope.ServiceProvider.GetRequiredService<IUserStoryImageRepository>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<StoryDeletionService>>();

                        var images = await storyRepository.GetAsync(new GetUserStoryRequest
                        {
                            MaxCreationTime = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(),
                            SkipPagination = true,
                        });

                        foreach(var image in images)
                        {
                            await mediator.Send(new DeleteUserStoryImageCommand { Id = image.Id });
                        }

                        logger.LogInformation("Deleted {count} story images - {date}", images.Count(), DateTime.Now);
                    }

                    await Task.Delay(Delay);
                }
            });
        }
    }
}
