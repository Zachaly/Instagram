using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile
{
    public static class BuilderExtensions
    {
        public static void AddServices(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ProfilePageViewModel>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<ShellViewModel>();
            builder.Services.AddTransient<PostPage>();
            builder.Services.AddTransient<PostPageViewModel>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<SearchPageViewModel>();
            builder.Services.AddTransient<App>();

            builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();

            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IUserFollowService, UserFollowService>();
            builder.Services.AddSingleton<IPostService, PostService>();
            builder.Services.AddSingleton<IPostCommentService, PostCommentService>();
        }
    }
}
